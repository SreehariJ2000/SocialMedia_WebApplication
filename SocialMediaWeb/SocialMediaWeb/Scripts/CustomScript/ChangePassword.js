$('#NewPassword').on('input', function () {
    validatePassword();
});


$('#UserPassword').on('input', function () {
    validateOldPassword();
});


$('#ConfirmPassword').on('input', function () {
    validateConfirmPassword();
});



function validateOldPassword() {
    var password = $('#UserPassword').val().trim();
    var isValid = true;

    $('#UserPasswordError').text('');

    if (password === "") {
        $('#UserPasswordError').text('Password is required.');
        isValid = false;
    } 
    return isValid;
}

function validatePassword() {
    var password = $('#NewPassword').val().trim();
    var passwordRegex = /^(?=.*[A-Za-z])(?=.*\d)(?=.*[!@#$%^&*()])[A-Za-z\d!@#$%^&*()]{8,}$/;
    var isValid = true;

    $('#NewPasswordError').text('');

    if (password === "") {
        $('#NewPasswordError').text('Password is required.');
        isValid = false;
    } else if (!passwordRegex.test(password)) {
        $('#NewPasswordError').text('Password must be at least 8 characters and contain letters and numbers.');
        isValid = false;
    }

    return isValid;
}

function validateConfirmPassword() {
    var password = $('#NewPassword').val().trim();
    var confirmPassword = $('#ConfirmPassword').val().trim();
    var isValid = true;

    $('#ConfirmPasswordError').text('');

    if (confirmPassword === "") {
        $('#ConfirmPasswordError').text('Please confirm your password.');
        isValid = false;
    } else if (confirmPassword !== password) {
        $('#ConfirmPasswordError').text('Passwords do not match.');
        isValid = false;
    }

    return isValid;
}

$('#submitBtn').on('click', function (e) {
    let isPassword= validatePassword();
    let isConfirmPassword = validateConfirmPassword();
    let isOldPassword = validateOldPassword()
    if (!isPassword || !isConfirmPassword || !isOldPassword) {
        e.preventDefault();
    }
});
    