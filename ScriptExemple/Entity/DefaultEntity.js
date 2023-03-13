var bufs = new Set();

function addBuf(buf) {
    bufs.add(buf);
}
function removeBuf(buf) {
    bufs.delete(buf);
}

function onTick() {
    for(let b in bufs) {
        if(b != undefined)
            b.onTick(GetAPIBase());
    }
}

function tryMove(X,Y) {
    let tile = GetModel().InvokeAction("GetTile",[X,Y]);

    if(!tile.InvokeAction("tryEnter",GetAPIBase()))
        return;
    
    tile.InvokeAction("onEnter",GetAPIBase());
}