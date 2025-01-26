/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID EXIT_WATER = 387843989U;
        static const AkUniqueID PLAY_AMBIENCE = 278617630U;
        static const AkUniqueID PLAY_BREATHING = 4093367312U;
        static const AkUniqueID PLAY_CLOTH = 128342314U;
        static const AkUniqueID PLAY_COMPASS_PICKUP = 1518103445U;
        static const AkUniqueID PLAY_COMPASS_SPIN = 1046130049U;
        static const AkUniqueID PLAY_CREATURE = 2891509749U;
        static const AkUniqueID PLAY_DOOR_LOOP = 2463718247U;
        static const AkUniqueID PLAY_FLUORESCENT_LIGHT = 3799125421U;
        static const AkUniqueID PLAY_GENERAL_FOOTSTEP_CONTAINER = 2589019387U;
        static const AkUniqueID PLAY_HEARTBEAT = 3765695918U;
        static const AkUniqueID PLAY_JUMPSCARE = 3840943850U;
        static const AkUniqueID PLAY_SFX_BOTTLE_OPEN = 605122265U;
        static const AkUniqueID PLAY_SFX_BOTTLE_PICK_UP = 2888013566U;
        static const AkUniqueID PLAY_SWIM = 165772206U;
        static const AkUniqueID PLAY_TRAIN_STATION_SONG = 3565446547U;
        static const AkUniqueID PLAY_TRANSITION = 895718239U;
        static const AkUniqueID PLAY_WALK = 1589278981U;
        static const AkUniqueID STOP_HEARTBEAT = 3319673256U;
        static const AkUniqueID STOP_SWIM = 127261724U;
        static const AkUniqueID STOP_WALK = 3140964691U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace MOVEMENT
        {
            static const AkUniqueID GROUP = 2129636626U;

            namespace STATE
            {
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID SPRINT = 1296465089U;
                static const AkUniqueID WALK = 2108779966U;
            } // namespace STATE
        } // namespace MOVEMENT

        namespace ROOM
        {
            static const AkUniqueID GROUP = 2077253480U;

            namespace STATE
            {
                static const AkUniqueID FINAL = 565529991U;
                static const AkUniqueID INTERMEDIATE = 2188788306U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID SUBWAY = 1981184526U;
                static const AkUniqueID SUBWAY_FLOOD = 1392138151U;
            } // namespace STATE
        } // namespace ROOM

        namespace STRESS
        {
            static const AkUniqueID GROUP = 3840192365U;

            namespace STATE
            {
                static const AkUniqueID LARGE = 4284352190U;
                static const AkUniqueID MED = 981339021U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID SMALL = 1846755610U;
            } // namespace STATE
        } // namespace STRESS

        namespace SURFACE
        {
            static const AkUniqueID GROUP = 1834394558U;

            namespace STATE
            {
                static const AkUniqueID CARPET = 2412606308U;
                static const AkUniqueID GRASS = 4248645337U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID SHALLOW_WATER = 2789056291U;
                static const AkUniqueID TILE = 2637588553U;
                static const AkUniqueID TILE_WET = 3922274248U;
                static const AkUniqueID WATER = 2654748154U;
            } // namespace STATE
        } // namespace SURFACE

    } // namespace STATES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID WETNESS = 3286765786U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID AMBIENT = 77978275U;
        static const AkUniqueID MASTER = 4056684167U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
