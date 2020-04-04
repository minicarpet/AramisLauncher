import crafttweaker.item.IItemStack;
import crafttweaker.item.IIngredient;


var itemArray as IItemStack[] = [
    <iceandfire:dragonforge_fire_brick>,
    <iceandfire:dragonforge_fire_input>,
    <iceandfire:dragonforge_fire_core>,
    <iceandfire:dragonsteel_fire_block>,
];

for item in itemArray {
    # The most common way of recipe
    recipes.remove(item);
}

/*
///////////////////////////////////////////////////////////////
Ice and Fire
///////////////////////////////////////////////////////////////
*/

	recipes.remove(<iceandfire:dragonforge_fire_brick>);
	recipes.remove(<iceandfire:dragonforge_fire_input>);
	recipes.remove(<iceandfire:dragonforge_fire_core_disabled>);
	recipes.remove(<iceandfire:dragonsteel_fire_block>);

	recipes.remove(<iceandfire:dragonforge_ice_brick>);
	recipes.remove(<iceandfire:dragonforge_ice_input>);
	recipes.remove(<iceandfire:dragonforge_ice_core_disabled>);
	recipes.remove(<iceandfire:dragonsteel_ice_block>);

mods.iceandfire.recipes.removeFireDragonForgeRecipe(<iceandfire:dragonsteel_fire_ingot>);
mods.iceandfire.recipes.removeIceDragonForgeRecipe(<iceandfire:dragonsteel_ice_ingot>);

	recipes.remove(<iceandfire:dragonsteel_fire_pickaxe>);
	recipes.remove(<iceandfire:dragonsteel_fire_axe>);
	recipes.remove(<iceandfire:dragonsteel_fire_shovel>);
	recipes.remove(<iceandfire:dragonsteel_fire_hoe>);
	recipes.remove(<iceandfire:dragonsteel_fire_sword>);

	recipes.remove(<iceandfire:dragonsteel_fire_helmet>);
	recipes.remove(<iceandfire:dragonsteel_fire_chestplate>);
	recipes.remove(<iceandfire:dragonsteel_fire_leggings>);
	recipes.remove(<iceandfire:dragonsteel_fire_boots>);

	recipes.remove(<iceandfire:dragonarmor_dragonsteel_fire:0>);
	recipes.remove(<iceandfire:dragonarmor_dragonsteel_fire:1>);
	recipes.remove(<iceandfire:dragonarmor_dragonsteel_fire:2>);
	recipes.remove(<iceandfire:dragonarmor_dragonsteel_fire:3>);

	recipes.remove(<iceandfire:dragonsteel_ice_pickaxe>);
	recipes.remove(<iceandfire:dragonsteel_ice_axe>);
	recipes.remove(<iceandfire:dragonsteel_ice_shovel>);
	recipes.remove(<iceandfire:dragonsteel_ice_hoe>);
	recipes.remove(<iceandfire:dragonsteel_ice_sword>);

	recipes.remove(<iceandfire:dragonsteel_ice_helmet>);
	recipes.remove(<iceandfire:dragonsteel_ice_chestplate>);
	recipes.remove(<iceandfire:dragonsteel_ice_leggings>);
	recipes.remove(<iceandfire:dragonsteel_ice_boots>);

	recipes.remove(<iceandfire:dragonarmor_dragonsteel_ice:0>);
	recipes.remove(<iceandfire:dragonarmor_dragonsteel_ice:1>);
	recipes.remove(<iceandfire:dragonarmor_dragonsteel_ice:2>);
	recipes.remove(<iceandfire:dragonarmor_dragonsteel_ice:3>);

	recipes.remove(<iceandfire:dragonarmor_iron:0>);
	recipes.remove(<iceandfire:dragonarmor_iron:1>);
	recipes.remove(<iceandfire:dragonarmor_iron:2>);
	recipes.remove(<iceandfire:dragonarmor_iron:3>);
	
	recipes.remove(<iceandfire:dragonarmor_gold:0>);
	recipes.remove(<iceandfire:dragonarmor_gold:1>);
	recipes.remove(<iceandfire:dragonarmor_gold:2>);
	recipes.remove(<iceandfire:dragonarmor_gold:3>);
	
	recipes.remove(<iceandfire:dragonarmor_diamond:0>);
	recipes.remove(<iceandfire:dragonarmor_diamond:1>);
	recipes.remove(<iceandfire:dragonarmor_diamond:2>);
	recipes.remove(<iceandfire:dragonarmor_diamond:3>);
	
	recipes.remove(<iceandfire:dragonarmor_silver:0>);
	recipes.remove(<iceandfire:dragonarmor_silver:1>);
	recipes.remove(<iceandfire:dragonarmor_silver:2>);
	recipes.remove(<iceandfire:dragonarmor_silver:3>);
	
	
/*
///////////////////////////////////////////////////////////////
Spartan fire
///////////////////////////////////////////////////////////////
*/

	recipes.removeByMod("spartanfire");
	
	recipes.remove(<spartanfire:longsword_ice_dragonsteel>);
	recipes.remove(<spartanfire:katana_ice_dragonsteel>);
	recipes.remove(<spartanfire:greatsword_ice_dragonsteel>);
	recipes.remove(<spartanfire:saber_ice_dragonsteel>);
	recipes.remove(<spartanfire:rapier_ice_dragonsteel>);
	recipes.remove(<spartanfire:dagger_ice_dragonsteel>);
	recipes.remove(<spartanfire:spear_ice_dragonsteel>);
	recipes.remove(<spartanfire:pike_ice_dragonsteel>);
	recipes.remove(<spartanfire:lance_ice_dragonsteel>);
	recipes.remove(<spartanfire:halberd_ice_dragonsteel>);
	recipes.remove(<spartanfire:warhammer_ice_dragonsteel>);
	recipes.remove(<spartanfire:hammer_ice_dragonsteel>);
	recipes.remove(<spartanfire:throwing_axe_ice_dragonsteel>);
	recipes.remove(<spartanfire:throwing_knife_ice_dragonsteel>);
	recipes.remove(<spartanfire:longbow_ice_dragonsteel>);
	recipes.remove(<spartanfire:crossbow_ice_dragonsteel>);
	recipes.remove(<spartanfire:javelin_ice_dragonsteel>);
	recipes.remove(<spartanfire:battleaxe_ice_dragonsteel>);
	recipes.remove(<spartanfire:boomerang_ice_dragonsteel>);
	recipes.remove(<spartanfire:mace_ice_dragonsteel>);
	recipes.remove(<spartanfire:staff_ice_dragonsteel>);
	recipes.remove(<spartanfire:glaive_ice_dragonsteel>);

	recipes.remove(<spartanfire:longsword_fire_dragonsteel>);
	recipes.remove(<spartanfire:katana_fire_dragonsteel>);
	recipes.remove(<spartanfire:greatsword_fire_dragonsteel>);
	recipes.remove(<spartanfire:saber_fire_dragonsteel>);
	recipes.remove(<spartanfire:rapier_fire_dragonsteel>);
	recipes.remove(<spartanfire:dagger_fire_dragonsteel>);
	recipes.remove(<spartanfire:spear_fire_dragonsteel>);
	recipes.remove(<spartanfire:pike_fire_dragonsteel>);
	recipes.remove(<spartanfire:lance_fire_dragonsteel>);
	recipes.remove(<spartanfire:halberd_fire_dragonsteel>);
	recipes.remove(<spartanfire:warhammer_fire_dragonsteel>);
	recipes.remove(<spartanfire:hammer_fire_dragonsteel>);
	recipes.remove(<spartanfire:throwing_axe_fire_dragonsteel>);
	recipes.remove(<spartanfire:throwing_knife_fire_dragonsteel>);
	recipes.remove(<spartanfire:longbow_fire_dragonsteel>);
	recipes.remove(<spartanfire:crossbow_fire_dragonsteel>);
	recipes.remove(<spartanfire:javelin_fire_dragonsteel>);
	recipes.remove(<spartanfire:battleaxe_fire_dragonsteel>);
	recipes.remove(<spartanfire:boomerang_fire_dragonsteel>);
	recipes.remove(<spartanfire:mace_fire_dragonsteel>);
	recipes.remove(<spartanfire:staff_fire_dragonsteel>);
	recipes.remove(<spartanfire:glaive_fire_dragonsteel>);
	
/*
///////////////////////////////////////////////////////////////
Botania
///////////////////////////////////////////////////////////////
*/


	mods.botania.RuneAltar.removeRecipe(<botania:rune>);
	mods.botania.RuneAltar.removeRecipe(<botania:rune:1>);
	mods.botania.RuneAltar.removeRecipe(<botania:rune:2>);
	mods.botania.RuneAltar.removeRecipe(<botania:rune:3>);

	mods.botania.RuneAltar.removeRecipe(<botania:rune:4>);
	mods.botania.RuneAltar.removeRecipe(<botania:rune:5>);
	mods.botania.RuneAltar.removeRecipe(<botania:rune:6>);
	mods.botania.RuneAltar.removeRecipe(<botania:rune:7>);

	mods.botania.RuneAltar.removeRecipe(<botania:rune:9>);
	mods.botania.RuneAltar.removeRecipe(<botania:rune:10>);
	mods.botania.RuneAltar.removeRecipe(<botania:rune:11>);
	mods.botania.RuneAltar.removeRecipe(<botania:rune:12>);
	mods.botania.RuneAltar.removeRecipe(<botania:rune:13>);
	mods.botania.RuneAltar.removeRecipe(<botania:rune:14>);
	mods.botania.RuneAltar.removeRecipe(<botania:rune:15>);
	
	mods.botania.RuneAltar.removeRecipe(<botania:rune:8>);
	mods.botania.RuneAltar.removeRecipe(<minecraft:skull:3>);

/*
///////////////////////////////////////////////////////////////
test
///////////////////////////////////////////////////////////////
*/

	recipes.removeByMod("netherrocks");
	recipes.removeByMod("simpleores");
	recipes.removeByMod("fusion");
	recipes.removeByMod("wings");
	recipes.removeByMod("embers");
	recipes.removeByMod("iceandfire");
	recipes.removeByMod("crystals_of_sao");
	recipes.removeByMod("waystones");





