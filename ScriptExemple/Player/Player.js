var bufs = new Set();

function addBuf(buf) {
    bufs.add(buf);
}
function removeBuf(buf) {
    bufs.delete(buf);
}

function onTick() {
    var base = GetAPIBase();
    base.Position = CreatePoint(base.Position.X,base.Position.Y);
    for(let b of bufs) {
        if(b != undefined)
            b.onTick(base);
    }
}

function tryMove(X,Y) {
    let tile = GetModel().InvokeAction("GetTile",[X,Y]);

    if(!tile.InvokeAction("onTryEnter",GetAPIBase()))
        return;
    
    var base = GetAPIBase();
    let pt = base.Position;
    base.Position = CreatePoint(X,Y);
    GetModel().Map.GetTile(pt.X,pt.Y).InvokeAction("onLeave",GetAPIBase());

    tile.InvokeAction("onEnter",GetAPIBase());
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
    for(let buf in Array.from(bufs))
    {
        val += buf.onGetName() + "\n\t";
        val += buf.onGetInfo() + "\n";
    }
    return {
        type: "string",
        value: val
    }
}