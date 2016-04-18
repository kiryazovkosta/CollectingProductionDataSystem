$(document).ready(function () {
    ChangeWidthNav();

    $(".help-side-nav-main").click(function () {
        $('#pageHelp-content').html("");
        AddOrRemoveActiveLi();
        ChangeWidthNav();
    });
    //-----------------------------------------------------------------------
    var array = ['collapseListGroup11', 'collapseListGroup12', 'collapseListGroup13', 'collapseListGroup14'];
    for (var k = 0; k < array.length; k++) {
        var newstr = "#".concat(array[k]).concat('>ul>li>a');
        $(newstr).click(function () {
            AddOrRemoveActiveLi();
            $(this).addClass("activeLi");
        });
    }
})

function AddOrRemoveActiveLi() {
    var array = [ 'collapseListGroup11','collapseListGroup12','collapseListGroup13','collapseListGroup14'];
    for (var j = 0; j < array.length; j++) {
        var newstring = "#".concat(array[j]).concat('>ul>li>a');
        var collection = $(newstring);
        for (var n = 0; n < collection.length; n++) {
            if ($(collection[n]).hasClass('activeLi')) {
                $(collection[n]).removeClass('activeLi');
            }
        }
    }
}
function ChangeWidthNav(){
   if ($('#navsitebar').width = '600px') {
      $('#ulnavsitebar').css('width', '213px');
   }
    else {
        $('#ulnavsitebar').css('width', '230px');
 }
}