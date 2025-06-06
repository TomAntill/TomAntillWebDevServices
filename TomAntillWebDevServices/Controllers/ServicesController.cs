﻿using FluentValidation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TomAntillWebDevServices.BLL.Contracts;
using TomAntillWebDevServices.Data.DataModels;
using TomAntillWebDevServices.Data.Enums;
using TomAntillWebDevServices.Models.ViewModels;
using TomAntillWebDevServices.Services.Contracts;
using TomAntillWebDevServices.Validation;

namespace TomAntillWebDevServices.Controllers
{
    [ApiController]
    [EnableCors("AllowSpecificOrigins")]

    [Route("api/services")]
    public class ServicesController : ControllerBase
    {
        private readonly IEmailBLL _emailService;
        private readonly IMediaService _mediaService;


        public ServicesController(IEmailBLL emailBLL, IMediaService mediaService)
        {
            _emailService = emailBLL;
            _mediaService = mediaService;
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<MediaVm> GetById(string appName, int id)
        {
            GetByIdValidator validator = new GetByIdValidator();
            validator.ValidateAndThrow(id);
            return await _mediaService.GetById(appName, id);
        }


        //Send Email
        [HttpPost]
        [Route("Add")]
        public async Task<string> Add(Email email)
        {
            return await _emailService.Add(email, email.WebsiteName);
        }

        [HttpPost]
        [Route("SendLogEmail")]
        public async Task<IActionResult> SendLogEmail()
        {
            var emailDataString = Request.Form["emailData"];

            if (string.IsNullOrEmpty(emailDataString))
                return BadRequest("Missing email data");

            var file = Request.Form.Files["attachment"];
            if (file == null || file.Length == 0)
                return BadRequest("Missing or empty attachment");

            var result = await _emailService.SendLogEmail(emailDataString, file);
            return Ok(result);
        }

        // get all pictures / get by category or project name
        [HttpGet]
        [Route("GetAll")]
        public async Task<List<MediaVm>> GetAll(string appName, UploadCategory? category = null, ProjectName? projectName = null)
        {
            return await _mediaService.GetAll(appName, category, projectName);
        }

        [HttpGet]
        [Route("GetAllProjects")]
        public async Task<List<MediaVm>> GetAllProjects(string appName)
        {
            var tsks = new List<Task<List<MediaVm>>>();
            ProjectName[] projects = (ProjectName[])Enum.GetValues(typeof(ProjectName));            
            foreach (var item in projects.Where(q => q != ProjectName.None))
            {
                var tsk = GetAll(appName, null, item);
                tsks.Add(tsk);
            }
            await Task.WhenAll(tsks);
            var results = tsks.SelectMany(sm => sm.Result).ToList();
            results.ForEach(fe => fe.TransfromPictureCategoryToProjectName());
            return results;
        }
    }
}
