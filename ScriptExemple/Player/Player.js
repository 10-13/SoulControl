var bufs = new Set();

function addUnEvalBuf(buf) {
    eval(buf);
    bufs.add(temp);
}
function addBuf(buf) {
    bufs.add(buf);
}
function removeBuf(buf) {
    bufs.delete(buf);
}

function onReset() {
    var t = GetAPIBase().GetType().GetProperties();
    for (let _t of t)
    {
        var f = _t.PropertyType.GetMethod("Restore");
        if(f != null)
            f.Invoke(_t.GetValue(GetAPIBase()), []);
    }
}
function onApply() {
    var Entity = GetAPIBase();
    for(let b of bufs) {
        if(b != undefined)
            b.onApply(Entity);
    }
}
function onTick() {
    var Entity = GetAPIBase();
    for(let b of bufs) {
        if(b != undefined)
            if(b.onTick(Entity))
                removeBuf(b);
    }
}

function tryMove(X,Y) {
    let tile = GetModel().InvokeAction("GetTile",[X,Y]);

    if(!tile.InvokeAction("onTryEnter",GetAPIBase()))
        return;
    
    var base = GetAPIBase();
    base.Position = CreatePoint(X,Y);
    
    tile.InvokeAction("onEnter",GetAPIBase());
}

function createTileFromColor(color)
{
    return `<div class="game-tile" style="background-color: ` + color +`"></div>`;
}
function createUndefTile(color)
{
    return `<div class="game-tile-undefiend" style="background-color: ` + color +`"></div>`;
}

//API -----------------------------------------------

function move(direction){
    var base = GetAPIBase();
    switch(direction) {
        case "up":
            tryMove(base.Position.X,base.Position.Y - 1);
            break;
        case "down":
            tryMove(base.Position.X,base.Position.Y + 1);
            break;
        case "left":
            tryMove(base.Position.X - 1,base.Position.Y);
            break;
        case "right":
            tryMove(base.Position.X + 1,base.Position.Y);
            break;
    }
}

function getBufs() {
    let val = "Bufs [" + bufs.size + "]:\n";
    for(let buf of bufs)
    {
        val += buf.onGetName() + "\n\t";
        val += buf.onGetInfo() + "\n";
    }
    return {
        type: "string",
        value: val
    }
}

function getInfo() {
    let val = "Stats:\n";
    val += "\nAir:   " + GetAPIBase().Air.Value.toString() + "\n";
    val += "\nMoves: " + GetAPIBase().Moves.Value.toString() + "\n";
    return {
        type: "string",
        value: val
    }
}

function scan(X,Y) {
    let field = `<div class="game-tilebox" id="game-container">\n`;
    for(let i = -5;i < 6;i++)
        for(let j = -5; j < 6;j++) {
            let tile = GetTile(X + j,Y + i);
            field += "\t";
            if(tile.TileType == "UNDEFINED")
                field += createUndefTile(tile.Color);
            else
                field += createTileFromColor(tile.Color);
            field += "\n";
        }
    return {
        type: "html",
        value: field
    }
}

function scan() {
    let X = GetAPIBase().Position.X;
    let Y = GetAPIBase().Position.Y;
    return scan(X,Y);
}


