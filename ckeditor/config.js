﻿/*
Copyright (c) 2003-2012, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function( config )
{
config.skin = 'v2';
config.toolbar = 'TadToolbar';
    config.toolbar_TadToolbar =
    [
        ['Source', 'Undo', 'Redo'],
        ['Bold', 'Italic', 'Underline'],
        ['JustifyLeft', 'JustifyCenter', 'JustifyRight'],
        ['PasteWord', 'Link', 'Image', 'Flash', 'MediaEmbed', 'Smiley', 'SpecialChar'],
    //'/',
    //['Font', 'FontSize', 'TextColor', 'BGColor', 'RemoveFormat']
        ['TextColor', 'RemoveFormat']
    ];
config.colorButton_enableAuto = false;
 config.enterMode = CKEDITOR.ENTER_BR;
    config.toolbar = 'TadToolbar';
    config.toolbarCanCollapse = false;
 config.scayt_autoStartup = false;
    config.pasteFromWordCleanupFile = true;
};

//test