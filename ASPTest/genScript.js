var tileRootPath = "C:\\Users\\Sleeper\\source\\repos\\ASPTest\\ScriptExemple\\Tiles\\";

function tilePath(name) {
	return tileRootPath + name + ".xml";
}

let startX = 0;
let startY = 0;
let width = 20;
let height = 20;

gen.LoadDrawTile(tilePath("EmptyTile"));
var empty = gen.DrawTile;
gen.LoadDrawTile(tilePath("DefaultTile"));
var field = gen.DrawTile;
gen.LoadDrawTile(tilePath("ForestTile"));
var forest = gen.DrawTile;

gen.LoadPlayer("C:\\Users\\Sleeper\\source\\repos\\ASPTest\\ScriptExemple\\Player\\Player.xml");


log(gen.Model.ActualField.TilesCollection.Length);

var drawFragment = function (x, y, w, h, tile) {
	for (let i = x - 1; i < x + w + 1; i++)
		if (gen.Model.ActualField.GetTile(i, y - 1).TileType == "UNDEFINED")
			gen.Model.ActualField.SetTile(i, y - 1, empty);
	for (let i = x - 1; i < x + w + 1; i++)
		if (gen.Model.ActualField.GetTile(i, y + h).TileType == "UNDEFINED")
			gen.Model.ActualField.SetTile(i, y + h, empty);
	for (let i = y - 1; i < y + h + 1; i++)
		if (gen.Model.ActualField.GetTile(x - 1, i).TileType == "UNDEFINED")
			gen.Model.ActualField.SetTile(x - 1, i, empty);
	for (let i = y - 1; i < y + h + 1; i++)
		if (gen.Model.ActualField.GetTile(x + w, i).TileType == "UNDEFINED")
			gen.Model.ActualField.SetTile(x + w, i, empty);
	gen.DrawTile = tile;
	gen.DrawMap(x, y, w, h);
}

var showFragment = function (x, y, w, h) {
	for (let j = y - 2; j < y + h + 2; j++) {
		for (let i = x - 2; i < x + w + 2; i++) {
			let ent = gen.Model.InvokeAction("FindEntity", [i, j]);
			if (ent != undefined)
				log('*');
			else if (gen.Model.ActualField.GetTile(i, j).TileType == "EMPTY")
				log("#");
			else if (gen.Model.ActualField.GetTile(i, j).TileType == "Field")
				log(".");
			else if (gen.Model.ActualField.GetTile(i, j).TileType == "River")
				log("~")
			else if (gen.Model.ActualField.GetTile(i, j).TileType == "Forest")
				log("%")
			else log(" ");
			if (gen.Model.Player.Position.X == i && gen.Model.Player.Position.Y == j)
				log('$')
			else
				log(" ");
		}
		log("\n");
	}
}

drawFragment(startX, startY, 200, 32, field);
drawFragment(0, 20, 55, 4, empty);
drawFragment(15, 5, 22, 15, forest);
drawFragment(15, 5, 8, 4, field);
drawFragment(42, 0, 28, 13, forest);
drawFragment(42, 9, 12, 7, field);
drawFragment(40, 12, 11, 6, forest);

showFragment(startX, startY, 80, 30);

/*
for(let i = 0; i < 20;i++) { 
	execute('move','down'); 
	execute('move','right');
}
for(let i = 0; i < 20;i++) { execute('move','down'); execute('move','right');}
execute('getInfo'); execute('getBufs');

*/
//execute('eval','Log(AnalyzeType(GetAPIBase()));')
//execute('eval','var e = CreateStringObject().AddProperty("pos",[11,2]).AddProperty("moveBase",15); Log(e.toString()); ')