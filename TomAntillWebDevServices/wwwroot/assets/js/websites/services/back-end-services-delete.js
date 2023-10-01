import * as BackendServicesHelpers from './back-end-services-helpers.js';
  
  export async function deleteImage(websiteName, id) {
    const token = BackendServicesHelpers.getCookieValue("token");
    const url = BackendServicesHelpers.setAPIUrl("delete");
    var finalUrl =
    url +
    "?websiteName=" +
    encodeURIComponent(websiteName) +
    "&id=" +
    encodeURIComponent(id);
  
    const xhr = new XMLHttpRequest();
  
    // Set the URL and method
    xhr.open("DELETE", finalUrl);
  
    // Set the callback function to handle the response
    xhr.onload = function () {
      if (xhr.status === 200) {
            // Request was successfulsaveTokenToCookie
          BackendServicesHelpers.changePath("Images", true);
      } else {
        // Request failed
        console.error("Image delete failed with status code " + xhr.status);
      }
    };
  
    // Set the Authorization header
    xhr.setRequestHeader("Authorization", "Bearer " + token);
  
    xhr.send();
  }