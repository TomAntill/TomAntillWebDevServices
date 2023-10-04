import BackendServices from '../back-end-services.js';

function handleFormSubmit(event) {
    event.preventDefault(); // Prevent the default form submission behavior
    const email = document.getElementById('emailInput').value;
    const password = document.getElementById('passwordInput').value;
    const websiteName = WEBSITE_SETTINGS.WEBSITE_ID;

    BackendServices.user.jwtLogin(email, password, websiteName)
        .then((response) => {
            BackendServices.helpers.changePath("Images");
        })
        .catch((error) => {
            console.error('Login failed:', error);

            localStorage.setItem("failureMessage", "Login failed. Please check your credentials.");

            const messagebox = document.getElementById("messagebox");
            messagebox.innerText = "Login failed. Please check your credentials.";
            messagebox.style.display = "block";

            setTimeout(fadeMyDiv, 3000);
        });
}

function fadeMyDiv() {
    $("#messagebox").fadeOut('slow');
    localStorage.removeItem("failureMessage");
}

const form = document.querySelector('.register-form');
form.addEventListener('submit', handleFormSubmit);