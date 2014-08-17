$(function ($) {

    $.supersized({

        // Functionality
        slide_interval: 8000,		// Length between transitions
        transition: 1, 			// 0-None, 1-Fade, 2-Slide Top, 3-Slide Right, 4-Slide Bottom, 5-Slide Left, 6-Carousel Right, 7-Carousel Left
        transition_speed: 700,		// Speed of transition

        // Components							
        slide_links: 'blank',	// Individual links for each slide (Options: false, 'num', 'name', 'blank')
        slides: [			// Slideshow Images
                                            { image: 'Images/1.jpg', title: 'Come Fly With Us! Please Join Us As We Take The Trip Of A Lifetime...', thumb: '', url: '' },
                                             { image: 'Images/2.jpg', title: 'Come Fly With Us! Please Join Us As We Take The Trip Of A Lifetime...', thumb: '', url: '' },
                                              { image: 'Images/3.jpg', title: 'Come Fly With Us! Please Join Us As We Take The Trip Of A Lifetime...', thumb: '', url: '' },
                                               { image: 'Images/4.jpg', title: 'Come Fly With Us! Please Join Us As We Take The Trip Of A Lifetime...', thumb: '', url: '' },
                                                { image: 'Images/5.jpg', title: 'Come Fly With Us! Please Join Us As We Take The Trip Of A Lifetime...', thumb: '', url: '' },
                                                 { image: 'Images/6.jpg', title: 'Come Fly With Us! Please Join Us As We Take The Trip Of A Lifetime...', thumb: '', url: '' },
                                                  { image: 'Images/7.jpg', title: 'Come Fly With Us! Please Join Us As We Take The Trip Of A Lifetime...', thumb: '', url: '' },
                                                   { image: 'Images/8.jpg', title: 'Come Fly With Us! Please Join Us As We Take The Trip Of A Lifetime...', thumb: '', url: '' },
                                                    { image: 'Images/9.jpg', title: 'Come Fly With Us! Please Join Us As We Take The Trip Of A Lifetime...', thumb: '', url: '' },
                                                     { image: 'Images/10.jpg', title: 'Come Fly With Us! Please Join Us As We Take The Trip Of A Lifetime...', thumb: '', url: '' }
                                          
        ]

    });
});

// hide #back-top first
$("#back-top").hide();
$(".view-container").hide();
// fade in #back-top
$(function () {

    $(window).scroll(function () {
        if ($(this).scrollTop() > 100) {
            $('#back-top').fadeIn();
        } else {
            $('#back-top').fadeOut();
        }
    });

    // scroll body to 0px on click
    $('#back-top a').click(function () {
        $('body,html').animate({
            scrollTop: 0
        }, 800);
        return false;
    });
});