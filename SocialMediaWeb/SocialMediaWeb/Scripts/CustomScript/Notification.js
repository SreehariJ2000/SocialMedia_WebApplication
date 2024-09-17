$(document).ready(function () {
    function fetchNotifications() {
        $.ajax({
            url: '/User/GetNotifications',
            type: 'GET',
            success: function (response) {
                var notificationList = $('#notificationList');
                notificationList.empty();
                console.log(response);

                if (response.length > 0) {
                    $.each(response, function (index, notification) {
                        var imageUrl = notification.ImageUrl
                            ? '<img src="data:image/jpeg;base64,' + notification.ImageUrl + '" alt="Post Image" class="notification-image" />'
                            : '';

                        var message = notification.Message || 'No message available';

                        notificationList.append('<li class="dropdown-item notification-item" id="notification-' + notification.NotificationId + '">' +
                            '<div class="notification-content">' +
                            imageUrl +
                            '<span class="notification-text">' + notification.messageTitle + '</span>' +
                            '</div>' +
                            '<button class="close-button" data-id="' + notification.NotificationId + '">&times;</button>' +
                            '</li>' +
                            '<h6 class="notification-message">' + message + '</h6>');
                    });
                    $('#notificationCount').text(response.length);
                } else {
                    notificationList.append('<li class="dropdown-item">No notifications</li>');
                }
            },
            error: function (error) {
                console.error('Error fetching notifications:', error);
            }
        });
    }


    $('#notificationList').on('click', '.close-button', function () {
        var button = $(this);
        var notificationId = button.data('id');
        var listItem = button.closest('li');


        $.ajax({
            url: '/User/UpdateViewStatus',
            type: 'POST',
            data: { notificationId: notificationId },
            success: function (response) {
                listItem.remove();
                $('#notificationCount').text($('#notificationList li').length);
            },
            error: function (error) {
                console.error('Error updating view status:', error);
            }
        });
    });

    // Fetch notifications every 5 seconds
    setInterval(fetchNotifications, 10000);
    fetchNotifications();

    // Fetch notifications when the dropdown is clicked
    $('#notificationDropdown').on('click', function () {
        fetchNotifications();
    });
});
