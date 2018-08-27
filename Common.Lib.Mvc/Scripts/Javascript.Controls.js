/* File Created: April 11, 2012 */

function GetAutoCompleteData(id, data, url) {
	$.ajax({
		type: "GET",
		url: url,
		contentType: "application/json; charset=utf-8",
		data: data,
		cache: false,
		async: false, // to set local variable
		success: function (availableOptions) {
			var appt = $('#' + id);
			appt.empty();
			$('#' + id).autocomplete({
				source: availableOptions
			});
		},
		error: function (jqXhr, error, errorThrown) {
			alert(jqXhr.responseText);
		}
	});
};

/*  
    Javascript helper that will get a JSON drop down list from
    the backend given the application id, field name, controller
    url, and a function to take the JSON data.
*/
function FillDropDownList(applicationId,fieldName,url,valueFunc) {

    $.get(url, { appID: applicationId, field: fieldName }, function (data) {
        valueFunc(data);
    }
    );
}
