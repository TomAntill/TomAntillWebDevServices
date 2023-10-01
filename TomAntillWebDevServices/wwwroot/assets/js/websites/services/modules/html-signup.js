import BackendServices from "../back-end-services.js";

BackendServices.user.isLoggedIn();

document
  .getElementById("signupForm")
  .addEventListener("submit", function (event) {
    event.preventDefault();

    const password = document.getElementById("password").value;
    const confirmPassword = document.getElementById("confirmPassword").value;

    if (password !== confirmPassword) {
      alert("Passwords do not match. Please confirm your password.");
      return;
    }
    // If passwords match, continue with signup process
    const email = document.getElementById("email").value;

    try {
      const response = BackendServices.user.jwtSignup(
        email,
        password,
        WEBSITE_SETTINGS.UNSET_WEBSITE_ID
      );
    } catch (error) {
      console.error("Signup error:", error);
    }
  });
