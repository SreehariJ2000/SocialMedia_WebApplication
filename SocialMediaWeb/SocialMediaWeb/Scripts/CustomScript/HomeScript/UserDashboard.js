
    $(document).ready(function () {
        $('.like-button').click(function () {
            var postId = $(this).data('post-id');
            var likeCountElement = $('#like-count-' + postId);

            $.post('/User/ToggleLikePost', { postId: postId }, function (response) {
                if (response.success) {
                    likeCountElement.text(response.likeCount);

                    var likeButton = $('#like-button-' + postId);
                    if (likeButton.data('liked')) {
                        likeButton.css('color', 'black').data('liked', false);
                    } else {
                        likeButton.css('color', 'blue').data('liked', true);
                    }
                }
            });
        });



        // load like status when page load
        // Iterate through all the posts to check the like status
        $('.like-button').each(function () {
            var postId = $(this).data('post-id');
            var likeButton = $(this);

            // Send AJAX request to check like status for each post
            $.post('/User/CheckLikeStatus', { postId: postId }, function (response) {
                if (response.liked) {
                    likeButton.css('color', 'blue').data('liked', true);  // Update the color if liked
                } else {
                    likeButton.css('color', 'black').data('liked', false);  // Keep black if not liked
                }
            });
        });
    });
