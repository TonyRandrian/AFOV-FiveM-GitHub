fx_version 'bodacious'
game 'gta5'

ui_page 'UI/dist/index.html'

file 'Client/bin/Release/**/publish/*.dll'

files {
    'UI/dist/index.html',
    'UI/dist/**/*'
}

client_script 'Client/bin/Release/**/publish/*.net.dll'
server_script 'Server/bin/Release/**/publish/*.net.dll'

author 'You'
version '1.0.0'
description 'Example Resource from C# Template'