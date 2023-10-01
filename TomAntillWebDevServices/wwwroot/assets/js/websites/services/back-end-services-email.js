import * as BackendServicesHelpers from './back-end-services-helpers.js';

export function sendMessage() {
    // Get form input values
    var name = document.getElementById("nameInput").value;
    var email = document.getElementById("emailInput").value;
    var message = document.getElementById("messageInput").value;
  
    // Create the email object
    var emailData = {
      Name: name,
      EmailAddress: email,
      Message: message,
    };
  
    // Make a POST request to your API endpoint
    fetch(BackendServicesHelpers.setAPIUrl("sendMessage"), {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(emailData),
    })
      .then(function (response) {
        if (response.ok) {
          // Email sent successfully
          alert("Email sent successfully!");
        } else {
          // Failed to send email
          alert("Failed to send email. Please try again later.");
        }
      })
      .catch(function (error) {
        // Error occurred during the API call
        alert(
          "An error occurred while sending the email. Please try again later."
        );
      });
  }