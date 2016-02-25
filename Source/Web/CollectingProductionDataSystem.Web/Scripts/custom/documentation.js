$(document).ready(function () {
    var array = ['collapseListGroupHeading', 'collapseListGroup'];
    for (var k = 0; k < array.length; k++) {
        var mainStr = array[k];
        for (var i = 11; i < 20; i++) {
            var anotherStr = mainStr.concat(i);
            if (document.getElementById(anotherStr)) {
                var el = document.getElementById(anotherStr).getElementsByTagName('a');
                for (var j = 0; j < el.length; j++) {
                    el[j].innerHTML = "<span class='marker'>►</span>" + el[j].innerHTML
                }
            }
       }
    }

    //-----------------------------------------------------------------------
    $("h4.panel-title").click(function () {
        $('#mypage-content').html("");
        AddOrRemoveActiveLi();
    });

    //-----------------------------------------------------------------------
    $('li.mylist-group-item > a').click(function () {
        var a = $('li.mylist-group-item > a')
        for (var i = 0; i < a.length; i++) {
            $(a[i]).children('span')[0].innerHTML = "►"
        }
        //-------------------------------------------------------------------------

        $('li.list-group-item.mylist-group-item').click(function () {
            AddOrRemoveActiveLi();
            $(this).addClass("activeLi");
        });
        //---------------------------------------------------------------------
        var text = $(this).children('span')[0].innerHTML
        if (text == "►") {
            $(this).children('span')[0].innerHTML = '▼';
        }
        else {
            $(this).children('span')[0].innerHTML = '►';
        }

    })
})

function AddOrRemoveActiveLi() {
    var collection = $('li.list-group-item.mylist-group-item');
    for (var i = 0; i < collection.length; i++) {
        if ($(collection[i]).hasClass('activeLi')) {
            $(collection[i]).removeClass('activeLi');
        }
    }
}