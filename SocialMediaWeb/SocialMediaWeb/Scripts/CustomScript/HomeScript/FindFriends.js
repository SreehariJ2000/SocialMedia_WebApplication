$(document).ready(function () {
    function searchFriends(searchProfile) {
        $.ajax({
            url: '/User/SearchFriends',
           

            data: { searchTerm: searchProfile },
            success: function (data) {
                var resultsHtml = '<table><thead><tr><th>Profile Picture</th><th>Name</th><th>Action</th></tr></thead><tbody>';
                console.log(data);
                $.each(data, function (index, profile) {
                    console.log(profile.ProfilePicture)
                    resultsHtml += '<tr>' +
                        '<td>' + (profile.ProfilePicture ? '<img src="' + profile.ProfilePicture + '" alt="Profile Picture" width="50px" height="50px"/>' : 'No Image') + '</td>' +
                        '<td>' + profile.FirstName + ' ' + profile.LastName + '</td>' +
                        '<td><a href="/User/ViewFriendsProfile?userId=' + profile.UserID + '" >View Profile</a></td>' +
                        '</tr>';
                });
                resultsHtml += '</tbody></table>';
                $('#searchResults').html(resultsHtml);
            },
            error: function () {
                $('#searchResults').html('<p>An error occurred while searching.</p>');
            }
        });
    }

    $('#searchInput').on('input', function () {
        var searchProfile = $(this).val();
        searchFriends(searchProfile);
    });
});