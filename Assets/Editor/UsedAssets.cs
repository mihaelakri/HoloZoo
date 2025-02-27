public static class UsedAssets
{
    public static readonly string prefix = "Assets/ANIMALS FULL PACK/";
    public static readonly string[] assets = {
        $"{prefix}African Animals Pack/Elephant/Prefabs/Elephant_Legacy.prefab",

        $"{prefix}African Animals Pack/Crocodile/Prefabs/Crocodile_Legacy.prefab",

        $"{prefix}African Animals Pack/Hippopotamus/Prefabs/Hippopotamus_PBR.prefab",

        $"{prefix}African Animals Pack/Lion and Lioness/Prefabs/Lioness_Legacy.prefab",
        $"{prefix}African Animals Pack/Lion and Lioness/Prefabs/Lion_Legacy.prefab",

        $"{prefix}Birds Pack/Golden Eagle/Prefabs/Eagle_Legacy.prefab",

        $"{prefix}Ocean Animals Pack Vol 1/Dolphin/Prefab/Dolphin.prefab",

        $"{prefix}Ocean Animals Pack Vol 1/Great White Shark/Prefab/GreatWhiteShark.prefab",

        $"{prefix}African Animals Pack/Zebra/Prefabs/Zebra_Legacy.prefab",

        $"{prefix}African Animals Pack/Rhinoceros/Prefabs/Rhinoceros_Legacy.prefab",

        $"{prefix}Forest Animals Pack/Boar/Prefabs/Boar_Legacy.prefab",

        $"{prefix}Forest Animals Pack/Bear/Prefabs/Bear_Legacy.prefab",

        $"{prefix}Forest Animals Pack/Deer/Stag/Prefabs/DeerStag_Legacy.prefab",

        $"{prefix}Forest Animals Pack/Wolf/Prefabs/WolfArctic_PBR.prefab",
        $"{prefix}Forest Animals Pack/Wolf/Prefabs/Wolf_Legacy.prefab",

        $"{prefix}Forest Animals Pack/Rabbit/Prefabs/Rabbit_Legacy.prefab",

        $"{prefix}Birds Pack/Crow/Prefabs/Crow_Legacy.prefab",

        $"{prefix}Farm Animals Pack/Goat/Prefabs/Goat_Legacy.prefab",

        $"{prefix}Ocean Animals Pack Vol 1/Humboldt Squid/Prefab/HumboldtSquid.prefab",

        $"{prefix}Ocean Animals Pack Vol 1/Leatherback Sea Turtle/Prefab/LeatherbackSeaTurtle.prefab",
   };

    // animations must be in the transition order as they are in the animation controller
    public static readonly string[][] animations = {
        new string[] {
            $"{prefix}African Animals Pack/Elephant/FBX Files/Elephant@IdleBreathe.FBX",
            $"{prefix}African Animals Pack/Elephant/FBX Files/Elephant@IdleLookAround.FBX",
            $"{prefix}African Animals Pack/Elephant/FBX Files/Elephant@IdleLookAroundEat.FBX",
            $"{prefix}African Animals Pack/Elephant/FBX Files/Elephant@IdleChew.FBX",
        },
        new string[] {
            $"{prefix}African Animals Pack/Crocodile/FBX Files/Crocodile@Idle.FBX",
            $"{prefix}African Animals Pack/Crocodile/FBX Files/Crocodile@IdleRest1.FBX",
            $"{prefix}African Animals Pack/Crocodile/FBX Files/Crocodile@IdleRest2.FBX",
            $"{prefix}African Animals Pack/Crocodile/FBX Files/Crocodile@IdleRest3.FBX",
        },
        new string[] {
            $"{prefix}African Animals Pack/Hippopotamus/FBX Files/Hippopotamus@IdleBreathe.FBX",
            $"{prefix}African Animals Pack/Hippopotamus/FBX Files/Hippopotamus@IdleLookAroundAndEat.FBX",
            $"{prefix}African Animals Pack/Hippopotamus/FBX Files/Hippopotamus@IdleChew.FBX",
        },
        new string[] {
            $"{prefix}African Animals Pack/Lion and Lioness/FBX Files/Lion@IdleBreathe.FBX",
        },
        new string[] {
            $"{prefix}Birds Pack/Golden Eagle/FBX Files/Eagle@Glide.FBX",
        },
        new string[] {
            $"{prefix}Ocean Animals Pack Vol 1/Dolphin/FBX Files/Dolphin@SwimForward.fbx",
        },
        new string[] {
            $"{prefix}Ocean Animals Pack Vol 1/Great White Shark/FBX Files/GreatWhiteShark@SwimForward.fbx",
        },
        new string[] {
            $"{prefix}African Animals Pack/Zebra/FBX Files/Zebra@IdleBreathe.FBX",
        },
        new string[] {
            $"{prefix}African Animals Pack/Rhinoceros/FBX Files/Rhinoceros@IdleBreathe.FBX",
            $"{prefix}African Animals Pack/Rhinoceros/FBX Files/Rhinoceros@IdleLookForFood.FBX",
            $"{prefix}African Animals Pack/Rhinoceros/FBX Files/Rhinoceros@IdleGraze.FBX",
            $"{prefix}African Animals Pack/Rhinoceros/FBX Files/Rhinoceros@IdleChew.FBX",
        },
        new string[] {
            $"{prefix}Forest Animals Pack/Boar/FBX Files/Boar@IdleBreathe.FBX",
            $"{prefix}Forest Animals Pack/Boar/FBX Files/Boar@IdleLookAround.FBX",
        },
        new string[] {
            $"{prefix}Forest Animals Pack/Bear/FBX Files/Bear@Idle4Legs.FBX",
        },
        new string[] {
            $"{prefix}Forest Animals Pack/Deer/Stag/FBX Files/DeerStag@IdleBreathe.FBX",
            $"{prefix}Forest Animals Pack/Deer/Stag/FBX Files/DeerStag@IdleLookAround.FBX",
            $"{prefix}Forest Animals Pack/Deer/Stag/FBX Files/DeerStag@IdleGraze.FBX",
            $"{prefix}Forest Animals Pack/Deer/Stag/FBX Files/DeerStag@IdleChew.FBX",
        },
        new string[] {
            $"{prefix}Forest Animals Pack/Wolf/FBX Files/Wolf@IdleBreathe.FBX",
            $"{prefix}Forest Animals Pack/Wolf/FBX Files/Wolf@IdleLookAround.FBX",
        },
        new string[] {
            $"{prefix}Forest Animals Pack/Rabbit/FBX Files/Rabbit@IdleLieDown.FBX",
            $"{prefix}Forest Animals Pack/Rabbit/FBX Files/Rabbit@IdleSniffleAround.FBX",
            $"{prefix}Forest Animals Pack/Rabbit/FBX Files/Rabbit@IdleSatBreathe.FBX",
        },
        new string[] {
            $"{prefix}Birds Pack/Crow/FBX Files/Crow@Glide.FBX",
        },
        new string[] {
            $"{prefix}Farm Animals Pack/Goat/FBX Files/Goat@IdleBreathe.FBX",
        },
        new string[] {
            $"{prefix}Ocean Animals Pack Vol 1/Humboldt Squid/FBX Files/HumboldtSquid@AttackPrey.fbx",
            // $"{prefix}Ocean Animals Pack Vol 1/Humboldt Squid/FBX Files/HumboldtSquid@SwimForward.fbx",
        },
        new string[] {
            $"{prefix}Ocean Animals Pack Vol 1/Leatherback Sea Turtle/FBX Files/LeatherbackSeaTurtle@SwimForward.fbx",
        }
   };
}