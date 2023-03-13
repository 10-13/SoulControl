//HANDLED API
{
    //[INVOKEACTION (name,...params)] cs call
    InvokeAction(name,args[]) {}

    //[GETMODEL ()] cs call
    GetModel() {}
}
//MODEL API {for all elements}
{
    //[FINDENTYTI (X,Y)] cs call
    FindEntity(X, Y) {} // -- returns Entity

    //[GETTILE (X,Y)] cs call
    GetTile(X, Y) {}    // -- returns Tile
}
//static class - TILE API
{
    //[COLOR get/set] cs call
    getColor() {}      // -- returns css representation of color
    setColor(value) {}

    //[TILETYPE get/set] cs call
    getType() {}       // -- returns tileType
    setType(value) {}
}
//static class - ENTITY API
{
    //[POSITION get/set] cs call
    getPosition() {}
    setPosition(value) {}

    //[COLLIDE get/set] cs call
    getColide() {}
    setColide(value) {}
}
//static class - NPC API
{
    //[RELATIONSHIP get/set] cs call
    getRelationship() {}
    setRelationship(value) {}

    //[REQIREDRELATIONSHIP get/set] cs call
    getReqiredRelationship() {}
    setReqiredRelationship(value) {}
}