﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function ShowImageFromUpload(input, imgId) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#' + imgId).attr('src', e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
    else {
        alert('select a file to see preview');
        $('#' + imgId).attr('src', '');
    }
}

