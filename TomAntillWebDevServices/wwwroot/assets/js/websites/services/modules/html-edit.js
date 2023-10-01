import BackendServices from "../back-end-services.js";

BackendServices.user.isLoggedIn();

const imageElement = document.getElementById("imageElement");
const imageNameElement = document.getElementById("imageName");
const uploadCategoryElement = document.getElementById("uploadCategory");
const projectNameElement = document.getElementById("projectName");

populateDetails();
var root = BackendServices.helpers.getCurrentHost();


async function populateDetails() {
  const res = await BackendServices.get.getById(
    WEBSITE_SETTINGS.WEBSITE_ID,
    BackendServices.helpers.getIdFromUrl()
  );

  imageElement.src = res.url;

  uploadCategoryElement.value = res.pictureCategory;
  projectNameElement.value = res.projectName;

  imageNameElement.value = res.name;

}

function GoToDetails() {
    var baseUrl = `${root}/Details?Id=`;
  var finalUrl = baseUrl + BackendServices.helpers.getIdFromUrl();
  window.location.replace(finalUrl);
}

function UpdateDetails() {
  var pictureCategory = document.getElementById("uploadCategory");
    var projectName = document.getElementById("projectName");
  var outProjectName = parseInt(projectName.value);
    var outUploadCategory = parseInt(UploadCategoryEnum(pictureCategory.value));

  var id = BackendServices.helpers.getIdFromUrl();

  var name = document.getElementById("imageName").value;

  BackendServices.post.editImage(
    id,
    name,
    WEBSITE_SETTINGS.WEBSITE_ID,
    outUploadCategory,
    outProjectName
  );
  GoToDetails();
}

function UploadCategoryEnum(pictureCategory) {
  if (pictureCategory == "BespokeCarpentry") return 0;
  if (pictureCategory == "ConcreteTops") return 1;
  if (pictureCategory == "Furniture") return 2;
  if (pictureCategory == "None") return 3;
}
var updateButton = document.getElementById("updateButton");
updateButton.addEventListener("click", UpdateDetails);

var returnToDetailsButton = document.getElementById("returnToDetailsButton");
returnToDetailsButton.addEventListener("click", GoToDetails);

var logoutButton = document.getElementById("logoutButton");
logoutButton.addEventListener("click", BackendServices.user.logout);
