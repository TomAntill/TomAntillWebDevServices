  export function setAPIUrl(action) {
    let baseUrl = `${WEBSITE_SETTINGS.BACKEND_SERVICES_BASE_PATH}api`;
    let endpoint;

    // Set the endpoint based on the action
    switch (action) {
      case "addImage":
        endpoint = "/admin/AddImage";
        break;
      case "getAll":
        endpoint = "/admin/GetAll";
        break;
      case "getById":
        endpoint = "/admin/GetById";
        break;
      case "delete":
        endpoint = "/admin/Delete";
        break;
      case "update":
        endpoint = "/admin/Update";
        break;
      case "JwtLogin":
        endpoint = "/auth/JwtLogin";
        break;
      case "JwtSignup":
        endpoint = "/auth/AddSystemUser";
            break;
        case "sendMessage":
            endpoint = "/services/Add";
            break;
        case "getAllImages":
            endpoint = "/services/GetAll"
            break;
      default:
        // Handle any other actions or set a default endpoint
        endpoint = "";
        break;
    }
    // Combine the base URL and endpoint to form the final URL
    let apiUrl = baseUrl + endpoint;
  
    // Use the apiUrl variable wherever you need it in your code
    return apiUrl;
  }

  export function getCookieValue(cookieName) {
    const name = cookieName + "=";
    const decodedCookie = decodeURIComponent(document.cookie);
    const cookieArray = decodedCookie.split(";");
  
    for (let i = 0; i < cookieArray.length; i++) {
      let cookie = cookieArray[i].trim();
      if (cookie.indexOf(name) === 0) {
        let cookieValue = cookie.substring(name.length, cookie.length);
        if (cookieValue.startsWith('"') && cookieValue.endsWith('"')) {
          cookieValue = cookieValue.slice(1, -1); // Remove double quotes if present
        }
        // Remove "Bearer" prefix if present
        if (cookieValue.startsWith("Bearer ")) {
          cookieValue = cookieValue.replace("Bearer ", "");
        }
        return cookieValue;
      }
    }
  
    return null; // Return null if the cookie is not found
  }
  
  export function getIdFromUrl() {
  const urlParams = new URLSearchParams(window.location.search);
  const id = urlParams.get('Id');
  return id;
}

export function changePath(newPath, clearParams = false) {
    // Get the current URL components
    const currentURL = window.location.href;
    const url = new URL(currentURL);

    // Update the path with the new path
    url.pathname = newPath;

    if (clearParams) {
        url.search = '';
    }

    // Set the new URL as the location
    window.location.href = url.href;
}

export function getCurrentHost() {
    const currentURL = window.location.href;
    const url = new URL(currentURL);
    const protocol = url.protocol;
    const hostname = url.hostname;
    const port = url.port;

    const result = `${protocol}//${hostname}${port ? `:${port}` : ''}`;

    return result;
}