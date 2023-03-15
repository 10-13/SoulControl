var MoveMode = 0x1100;
var Air = 1;

function getMoveCallback() {
    return MoveMode;
}

function onTryEnter(Entity) {
    return (Entity.MoveMode.Value & MoveMode) != 0;
}

function onEnter(Entity) {
    if(Entity.GetType().Name == "Player" && (Entity.MoveMode & 0x1000) == 0) {
        let s = CreateStringObject().AddProperty("pos", [Entity.Position.X,Entity.Position.Y]);
        s.AddFunction("onApply", function(_Entity) {
            _Entity.GetValue('Moves').Value *= 0.8;
        });
        s.AddFunction("onTick", function(_Entity) {
            if(_Entity.GetValue('Position').X != this.pos[0] || _Entity.GetValue('Position').Y != this.pos[1])
                return true;
            return false;
        });
        s.AddFunction("onGetName", function(){ return "Slowed"; }).AddFunction("onGetInfo",function(){ return "Slowed in forest to 0.8"; })
        Log(s.toString());
        Entity.InvokeAction("addUnEvalBuf" ,s.toString());
    }
}

function onStay(Entity) {

}

function onLeave(Entity) {
    
}

function onGetName() {
    return GetAPIBase().TileType;
}

function onGetInfo() {
    return GetAPIBase().TileType;
}