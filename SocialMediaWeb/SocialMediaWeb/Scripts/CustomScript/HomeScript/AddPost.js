 $(document).ready(function () {
        
        $('#submitBtn').on('click', function (e) {
            let isValid = true;
            let content = $('#Content').val().trim();
            let fileInput = $('#ImageFile');
            let file = fileInput[0].files[0];
            let allowedExtensions = /(\.jpg|\.jpeg|\.png|\.gif)$/i;
    
            $('.error-message').text('');
         
            if (content === "") {
                $('#ContentError').text('Please add your content.');
                isValid = false;
            }

            
            if (file) {
                if (!allowedExtensions.exec(fileInput.val())) {
                    $('#ImageError').text('Only image files (JPG, PNG, GIF) are allowed.');
                    isValid = false;
                }
            }
            
            if (!isValid) {
                e.preventDefault();
            }
        });
   
    $('#Content').on('input', function () {
            if ($(this).val().trim() !== "") {
        $('#ContentError').text('');
            }
        });

    $('#ImageFile').on('change', function () {
        let fileInput = $('#ImageFile');
    let file = fileInput[0].files[0];
    let allowedExtensions = /(\.jpg|\.jpeg|\.png|\.gif)$/i;

    if (file && allowedExtensions.exec(fileInput.val())) {
        $('#ImageError').text('');
            }
        });
    });