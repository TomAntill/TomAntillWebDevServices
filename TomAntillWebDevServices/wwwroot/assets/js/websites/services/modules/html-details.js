import BackendServices from "../back-end-services.js";

BackendServices.user.isLoggedIn();

const imageElement = document.getElementById("imageElement");
const imageNameElement = document.getElementById("imageName");
const uploadCategoryElement = document.getElementById("uploadCategory");
const projectNameElement = document.getElementById("projectName");

populateDetails();

var root = BackendServices.helpers.getCurrentHost();

function returnToList() {
    window.location.replace(`${root}/Images`);
}

function GoToDelete() {
    var baseUrl = `${root}/Delete?Id=`;
    var finalUrl = baseUrl + BackendServices.helpers.getIdFromUrl();
    window.location.replace(finalUrl);
}

function GoToEdit() {
    var baseUrl = `${root}/Edit?Id=`;
    var finalUrl = baseUrl + BackendServices.helpers.getIdFromUrl();
    window.location.replace(finalUrl);
}

async function populateDetails() {
  const res = await BackendServices.get.getById(
    WEBSITE_SETTINGS.WEBSITE_ID,
    BackendServices.helpers.getIdFromUrl()
  );
  imageElement.src = res.url;
  imageNameElement.textContent = res.name;
  uploadCategoryElement.textContent = await UploadCategoryEnum(
    res.pictureCategory
  );
  projectNameElement.textContent = await ProjectNameEnum(res.projectName);
}

function UploadCategoryEnum(pictureCategory) {
  if (pictureCategory == "BespokeCarpentry") return "Bespoke Carpentry";
  if (pictureCategory == "ConcreteTops") return "Concrete Tops";
  if (pictureCategory == "Furniture") return "Furniture";
  if (pictureCategory == "None") return "None";
}

function ProjectNameEnum(projectName) {
  if (projectName == 0) return "Tregonwell Road";
  if (projectName == 1) return "Ponsford Road";
  if (projectName == 2) return "Summerhouse";
  if (projectName == 3) return "None";
}

var updateButton = document.getElementById("updateButton");
updateButton.addEventListener("click", GoToEdit);

var returnToListButton = document.getElementById("returnToListButton");
returnToListButton.addEventListener("click", returnToList);

var deleteButton = document.getElementById("deleteButton");
deleteButton.addEventListener("click", GoToDelete);

var logoutButton = document.getElementById("logoutButton");
logoutButton.addEventListener("click", BackendServices.user.logout);
