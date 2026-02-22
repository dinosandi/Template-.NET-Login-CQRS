using System;
namespace ComponentManagement.Domain.Enums;

public enum ComponentStatus
{
     RFU, // READY FOR USE
     WIP,  //(Work In progress ( Rebuilding)) WORK IN PROGRESS
     WIP2, // WORK (Work In Progress (Need APL)) IN PROGRESS 2
     WIP3, // (Work In Progress (Need FR)) WORK IN PROGRESS 3
     Requested, //diminta install
     INSTALLED //sudah terpasang
}
