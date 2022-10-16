const loginButton = document.querySelector("#login-button");

loginButton.addEventListener((e) => {
    location.href = `/Login/Save?Email=${email}`;
});
