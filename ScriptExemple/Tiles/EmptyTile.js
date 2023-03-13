var MoveMode = 0x0000;
var Air = 0;

function getMoveCallback() {
    return MoveMode;
}

function onTryEnter(Entity) {
    return false;
}

function onGetName() {
    return GetAPIBase().TileType;
}

function onGetInfo() {
    return GetAPIBase().TileType;
}