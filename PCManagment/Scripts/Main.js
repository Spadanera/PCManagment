function ToggleMobileMenu() {
    var toggleMenu = $(".navbar-header > button.navbar-toggle");
    var toggleElement = $("div.navbar-collapse")
    if (toggleMenu.is(":visible")) {
        toggleElement.removeClass("in");
    }
}

function Load() {
    $("#bg_dialog").height($(window).height());
    $("#bg_dialog").width($(window).width());

    $(window).resize(function () {
        $("#bg_dialog").height($(window).height());
        $("#bg_dialog").width($(window).width());
    });
}

function ShowDialog(dialog) {
    var margin = $("#search").css('margin-top');
    var halfWindowHeight = $(window).height() / 2;
    if ((margin.replace('px', ' ') * (-1)) > halfWindowHeight)
        $("#search").css('margin-top', '-' + halfWindowHeight + 'px')
    $("#bg_dialog").show();
    $(dialog).fadeIn(500);
}

function HideDialog(dialog) {
    $(dialog).fadeOut();
    $("#bg_dialog").hide();
}

function toggleInputCheckBox(propertyName) {
    var input = $(propertyName);
    if (input.val() == 'true')
        input.val('false');
    else
        input.val("true");
}

function submitForm(submit) {
    $(submit).click();
}

function ShowToast(text, result) {
    $('#toast' + result).attr('text', text);
    document.querySelector('#toast' + result).open();
}

function ShowAlert() {
    var dialog = document.getElementById('alert');
    if (dialog) {
        dialog.toggle();
    }
}

function showAddExtraButton()
{
    $("#bg_dialog").css("z-index", 48).show().click(function() {
        hideAddExtraButton();
    });
    var positions = ["0", "75%", "65%", "55%", "45%", "35%"];
    $("paper-icon-button").each(function (index) {
        if (index != 0) {
            $(this).animate({ top: positions[index] }).click(function () {
                hideAddExtraButton();
            });
        }
        else
            $(this).css("z-index", 30);
    });
}

function hideAddExtraButton() {
    var positions = ["0", "75%", "65%", "55%", "45%", "35%"];
    $("paper-icon-button").each(function (index) {
        if (index != 0) {
            $(this).animate({ top: "85%" });
        }
        else
            $(this).css("z-index", 50);
    });
    $("#bg_dialog").css("z-index", 100).hide();
}