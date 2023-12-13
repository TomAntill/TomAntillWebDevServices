import BackendServices from '../back-end-services.js';

BackendServices.user.isLoggedIn();
console.log("1", WEBSITE_SETTINGS.WEBSITE_ID);

// Your JavaScript function 'addImage()' goes here.
async function addImage(name, file, websiteName, uploadCategory, projectName) {
  }
  // Handle form submission
  document.getElementById('uploadForm').addEventListener('submit', async (event) => {
    event.preventDefault();

    const name = document.getElementById('name').value;
    const file = document.getElementById('file').files[0];
    const uploadCategory = document.getElementById('uploadCategory')?.value;
    const projectName = document.getElementById('projectName')?.value;
    try {
        await BackendServices.post.addImage(name, file, WEBSITE_SETTINGS.WEBSITE_ID, uploadCategory, projectName);
    } catch (error) {
      console.error('Error:', error);
      localStorage.setItem("failureMessage", "Upload failed. Please check your credentials.");

      const messagebox = document.getElementById("messagebox");
      messagebox.innerText = "Upload failed.";
      messagebox.style.display = "block";

      setTimeout(fadeMyDiv, 3000);
    }

  });
  function fadeMyDiv() {
    $("#messagebox").fadeOut('slow');
    localStorage.removeItem("failureMessage");
}

var logoutButton = document.getElementById("logoutButton");
logoutButton.addEventListener("click", BackendServices.user.logout);