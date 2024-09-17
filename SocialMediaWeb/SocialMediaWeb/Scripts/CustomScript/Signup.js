 $(document).ready(function () {
            $('#StateDropdown').change(function () {
                var stateId = $(this).val();
                if (stateId) {
                    $.ajax({
                        url: '/Authentication/GetCities',

                        data: { stateId: stateId },
                        success: function (data) {
                            var districtDropdown = $('#DistrictDropdown');
                            districtDropdown.empty();
                            districtDropdown.append('<option value="">Select District</option>');
                            $.each(data, function (index, item) {
                                districtDropdown.append('<option value="' + item.Value + '">' + item.Text + '</option>');
                            });
                        }
                    });
                } else {
                    $('#DistrictDropdown').empty().append('<option value="">Select District</option>');
                }
            });


     // Functions
     const namePattern = /^[a-zA-Z\s]+$/;
     function validateName(inputId, errorId, fieldName) {
         let nameValue = $(inputId).val().trim();    
         let errorElement = $(errorId);

         if (nameValue === "") {
             errorElement.text(fieldName + ' is required.');
             return false;
         }
        
         else if (!namePattern.test(nameValue)) {
             errorElement.text(fieldName + ' must contain only alphabets.');
             return false;
         }
       
         errorElement.text('');
         return true;
     }


     //gender
     function validateGender() {
         let genderValue = $('input[name="Gender"]:checked').val();
         let errorElement = $('#GenderError');

         if (!genderValue) {
             errorElement.text('Gender is required.');
             return false;
         }

         errorElement.text('');
         return true;
     }


     //address
     function validateAddress() {
         var address = $('#Address').val().trim();
        
         var isValid = true;

         // Clear previous errors
         $('#AddressError').text('');

         // Check if address is empty
         if (address === "") {
             $('#AddressError').text('Address cannot be empty.');
             isValid = false;
         }

         return isValid;
     }


     //state city
     function validateState() {
         var state = $('#StateDropdown').val();
         var isValid = true;

         $('#StateError').text('');

         if (state === "" || state === null) {
             $('#StateError').text('Please select a state.');
             isValid = false;
         }

         return isValid;
     }

     function validateDistrict() {
         var district = $('#DistrictDropdown').val();
         var isValid = true;

         $('#DistrictError').text('');

         if (district === "" || district === null) {
             $('#DistrictError').text('Please select a district.');
             isValid = false;
         }

         return isValid;
     }


     function validateProfilePicture() {
         let isValid = true;
         let fileInput = $('#ProfilePicture');
         let file = fileInput[0].files[0];
         let allowedExtensions = /(\.jpg|\.jpeg|\.png|\.gif)$/i;

         $('#ProfilePictureError').text('');

         if (file) {
             if (!allowedExtensions.exec(file.name)) {
                 $('#ProfilePictureError').text('Only image files (JPG, PNG, GIF) are allowed.');
                 isValid = false;
             }
         } else {
             $('#ProfilePictureError').text('Please select a profile picture.');
             isValid = false;
         }

         return isValid;
     }


     function validateEmail() {
         var email = $('#Email').val().trim();
         var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
         var isValid = true;

         $('#EmailError').text('');

         if (email === "") {
             $('#EmailError').text('Email is required.');
             isValid = false;
         } else if (!emailRegex.test(email)) {
             $('#EmailError').text('Please enter a valid email address.');
             isValid = false;
         }

         return isValid;
     }

     // Function to validate PhoneNumber
     function validatePhoneNumber() {
         var phoneNumber = $('#PhoneNumber').val().trim();
         var phoneRegex = /^(?!(\d)\1{9})[6,7,8,9]\d{9}$/
         var isValid = true;

         $('#PhoneNumberError').text('');

         if (phoneNumber === "") {
             $('#PhoneNumberError').text('Phone number is required.');
             isValid = false;
         } else if (!phoneRegex.test(phoneNumber)) {
             $('#PhoneNumberError').text('Please enter a validnumber (10 digits) and start with (9,8,6).');
             isValid = false;
         }

         return isValid;
     }


     function validatePassword() {
         var password = $('#UserPassword').val().trim();
         var passwordRegex = /^(?=.*[A-Za-z])(?=.*\d)(?=.*[!@#$%^&*()])[A-Za-z\d!@#$%^&*()]{8,}$/;
         var isValid = true;

         $('#UserPasswordError').text('');

         if (password === "") {
             $('#UserPasswordError').text('Password is required.');
             isValid = false;
         } else if (!passwordRegex.test(password)) {
             $('#UserPasswordError').text('Password must be at least 8 characters long and contain both letters and numbers.');
             isValid = false;
         }

         return isValid;
     }

     function validateConfirmPassword() {
         var password = $('#UserPassword').val().trim();
         var confirmPassword = $('#confirm-password').val().trim();
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


     // Real-time validation 
     $('#FirstName').on('input', function () {
         validateName('#FirstName', '#FirstNameError', 'First Name');
     });

     $('#LastName').on('input', function () {
         validateName('#LastName', '#LastNameError', 'Last Name');
     });

     $('input[name="Gender"]').on('change', function () {
         validateGender();
     });

     $('#Address').on('input', function () {
         validateAddress();
     });

     $('#StateDropdown').on('change', function () {
         validateState();
     });

     $('#DistrictDropdown').on('change', function () {
         validateDistrict();
     });

     $('#ProfilePicture').on('change', function () {
         validateProfilePicture(); 
     });

     $('#Email').on('input', function () {
         validateEmail();
     });

     $('#PhoneNumber').on('input', function () {
         validatePhoneNumber();
     });

     $('#UserPassword').on('input', function () {
         validatePassword();
     });

     $('#confirm-password').on('input', function () {
         validateConfirmPassword();
     });

     // Form submission 
     $('#submitBtn').on('click', function (e) {
         let isFirstNameValid = validateName('#FirstName', '#FirstNameError', 'First Name');
         let isLastNameValid = validateName('#LastName', '#LastNameError', 'Last Name');
         let isGenderValid = validateGender();
         let isAddress = validateAddress();
         let isState = validateState();
         let isDistrict = validateDistrict();
         let isProfilePicture = validateProfilePicture();
         let isEmail = validateEmail();
         let isPhone = validatePhoneNumber();
         let isPassword = validatePassword();
         let isConfirmPassword = validateConfirmPassword();

         // Prevent form submission if validation fails
         if (!isFirstNameValid || !isLastNameValid || !isGenderValid || !isAddress || !isState || !isDistrict
             || !isProfilePicture || !isEmail || !isPhone || !isPassword || !isPassword || !isConfirmPassword) {
             e.preventDefault();
         }
     });


     // Disble future date
     var todayDate = new Date();
     var month = todayDate.getMonth() + 1; // Months are zero-based
     var year = todayDate.getFullYear();
     var tdate = todayDate.getDate();

     // Add leading zeros to month and date if needed
     if (month < 10) {
         month = "0" + month;
     }

     if (tdate < 10) {
         tdate = "0" + tdate;
     }

     // Create the maxDate string in the format yyyy-MM-dd
     var maxDate = year + "-" + month + "-" + tdate;

     // Set the min attribute of the date input field using jQuery
     $("#DateOfBirth").attr("max", maxDate);





     
   });
  