var MoveMode = 0x1100;
var Air = 1;

function getMoveCallback() {
    return MoveMode;
}

function onTryEnter(Entity) {
    if(Entity.MoveMode && MoveMode == 0)
        return false;
    return true;
}

function onStay(Entity) {
    if(Entity.AirReqired > Air)
        Entity.Air -= Entity.AirReqired - Air;
    if(Air < 0)
        Entity.HP -= Entity.AirReqired * 200;
}

function onLeave(Entity) {
    
}

function onGetName() {
    return GetAPIBase().TileType;
}

function onGetInfo() {
    return GetAPIBase().TileType;
}