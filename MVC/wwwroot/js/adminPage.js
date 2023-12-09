$(document).ready(
    function () {

       /* function loadPartialView(viewName) {
            $.get(`Data/${viewName}Partial`, function (data) {
                $(`.${viewName}Partial`).html(data).show(); 
            });
        }

        $(".first").on("click", function () {
            loadPartialView("_First");
        });*/

        $(".first").on("click", function () {
           //  loadPartialView("_First");
           $(".firstpartial").toggle();
            $(".SecondPartial").css({ 'display':"none"  });
            $(".ThirdPartial").css({ 'display': "none" });
        })
        $(".second").on("click",function () {
            $(".firstpartial").css({ 'display': "none"});
            $(".ThirdPartial").css({ 'display': "none"});
            $(".SecondPartial").toggle();
        })
        $(".third").on("click",function () {
            $(".firstpartial").css({ 'display': "none" });
            $(".SecondPartial").css({  'display': "none"});
            $(".ThirdPartial").toggle();
        })
})