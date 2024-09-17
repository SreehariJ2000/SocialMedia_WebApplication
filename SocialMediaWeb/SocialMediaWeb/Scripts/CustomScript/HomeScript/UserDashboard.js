
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



       
        $('.like-button').each(function () {
            var postId = $(this).data('post-id');
            var likeButton = $(this);
            $.post('/User/CheckLikeStatus', { postId: postId }, function (response) {
                if (response.liked) {
                    likeButton.css('color', 'blue').data('liked', true);  
                } else {
                    likeButton.css('color', 'black').data('liked', false);  
                }
            });
        });
    });
