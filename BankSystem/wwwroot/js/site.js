// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

jQuery(function ($) {
   
    $('.single-post').slick({
     
        lazyLoad: 'ondemand',
        dots: true,
        infinite: true,
        speed: 500,
        fade: true,
        cssEase: 'linear'
        
       
     
       
    });

    $('.variable-width').slick({
        dots: true,
        infinite: true,
        speed: 300,
        slidesToShow: 1,
        centerMode: true,
        variableWidth: true
    });

    

    $('.lazy').slick({
        lazyLoad: 'ondemand',
        slidesToShow: 2,
        slidesToScroll: 1,
        autoplay: true,
        autoplaySpeed: 2000,
        dots: true,
        infinite: true,
         
       
       
    });
});

function previewFile() {
    var preview = document.getElementById('previewImage');
    var file = document.getElementById('imageUpload').files[0];
    var reader = new FileReader();

    reader.onloadend = function () {
        preview.src = reader.result;
        preview.style.display = "block"; // عرض الصورة عند التحميل
    }

    if (file) {
        reader.readAsDataURL(file);
    }
}


