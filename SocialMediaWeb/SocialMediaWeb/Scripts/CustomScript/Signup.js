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
        });
  