/**
 * 说明，导出的地图文件没有使用图集，需要使用egret里面定义的图集将image索引更新一下
 *   > node export-maps.js
 */




let Path = require('path');
let FS = require( "fs" );


function egret( resFile, rSourceFile, rTargetFile )
{
    let res = require( resFile );
    if( null == res.resources)
    {
        return;
    }
    let rSource = require( rSourceFile );
    if( null == rSource)
    {
        return;
    }

    resSubkeyDic = {}
    resSheetDic = {}
    for( let resKey in res.resources )
    {
        let resIt = res.resources[ resKey ];
        if (resIt.type == "sheet")
        {
            resSheetDic[resIt.name] = resIt.subkeys.split(',');

            let subkeys = resSheetDic[resIt.name];
            for (let subKey in subkeys)
            {
                resSubkeyDic[subkeys[subKey]] = resIt.name + '.' + subkeys[subKey];
            }
        }
    }

    // FS.writeFileSync( rTargetFile, JSON.stringify( resSubkeyDic, null, "\t" ) );
    // console.log(JSON.stringify( resSheetDic, null, "\t" ));


    RepairNode(rSource, resSubkeyDic);


    FS.writeFileSync( rTargetFile, JSON.stringify( rSource, null, "\t" ) );
}

function RepairNode(node, resSubkeyDic)
{
    if (!node)
    {
        return;
    }

    if (node.image)
    {
        let realName = resSubkeyDic[node.image];
        if (realName)
        {
            node.image = realName;
        }
    }

    if (node.children)
    {
        for(let child in node.children)
        {
            RepairNode(node.children[child], resSubkeyDic);
        }
    }
}


egret("../FireBird/resource/default.res.json",
    "./Map1.json",
    "./export/Map1.json");

egret("../FireBird/resource/default.res.json",
    "./Map2.json",
    "./export/Map2.json");
	
egret("../FireBird/resource/default.res.json",
    "./Map3.json",
    "./export/Map3.json");