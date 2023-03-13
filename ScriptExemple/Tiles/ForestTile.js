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

function onEnter(Entity) {
    if(Entity.GetType().Name == "Player" && Entity.MoveMode && 0x1000 == 0) {
        Entity.InvokeAction("addBuf" ,{ 
            baseMoves: Entity.Moves,
            onGetName(){ return "Slowed"; },
            onGetInfo(){ return "Slowed in forest to 0.8"; },
            pos: [Entity.Position.X,Entity.Position.Y],
            onTick(_Entity) {
                Log(_Entity.Position.X.toString() + ":" +_Entity.Position.Y.toString());
                Log(this.pos[0] + ":" + this.pos[1]);
                if(_Entity.Position.X != this.pos[0] || _Entity.Position.Y != this.pos[1]) {
                    _Entity.Moves = baseMoves;
                    _Entity.InvokeAction("removeBuf",this);
                }
            }
        });
        Entity.Moves = 0.8 * Entity.Moves;
        Log("MovesChanged");
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