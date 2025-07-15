using MF2024_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models;
using System.Text;
using Device = MF2024_API.Models.Device;
using Room = MF2024_API.Models.Room;
using User = MF2024_API.Models.User;

namespace MF2024_API.Service
{
    public class SeetData
    {
        public static async Task SeetDataLoad(IServiceProvider serviceProvider)
        {
            using (var socop = serviceProvider.CreateScope())
            {
                var dbContext = new Mf2024apiDbContext();


                var services = socop.ServiceProvider;
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {

                        var context = services.GetRequiredService<Mf2024apiDbContext>();
                        var userManager = services.GetRequiredService<UserManager<User>>();
                        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                        context.Database.EnsureCreated();

                        // Create Roles
                        if (!context.Roles.Any())
                        {
                            var roles = new List<IdentityRole>
                                    {
                                        new IdentityRole { Name = "Admin" },
                                        new IdentityRole { Name = "User" },
                                        new IdentityRole { Name = "Chief" },
                                        new IdentityRole { Name = "Manager" },
                                        new IdentityRole { Name = "RoomDevice" },
                                        new IdentityRole { Name = "ConferencRoomDevice" },
                                        new IdentityRole { Name = "Web" },
                                        new IdentityRole { Name = "Reseption" },
                                        new IdentityRole { Name = "PubulicSpace" },
                                    };
                            foreach (var role in roles)
                            {
                                await roleManager.CreateAsync(role);
                            }
                            await context.SaveChangesAsync();
                        }
                        // Create User
                        if (!context.Users.Any())
                        {
                            //ユーザーの追加
                            var adduser = new User
                            {
                                UserName = "admin",
                                Email = "",
                                UserPasswoedUpdataTime = DateTime.Now,
                                UserDateOfBirth = DateTime.Now,
                                UserGender = "男",
                                UserAddress = "東京都",
                                UserDateOfEntry = DateTime.Now,
                                UserDateOfRetirement = null,
                                UserUpdataDate = DateTime.Now,
                                UserUpdataUser = "admin"
                            };
                            //var addRoomDeviceUser = new User
                            //{
                            //    UserName = "RoomDevice1",
                            //    UserAddress = "RoomDevice",
                            //    UserGender = "",
                            //    Email = "",
                            //};
                            //var PubulicSpaceUser1 = new User
                            //{
                            //    UserName = "PubulicSpace1",
                            //    UserAddress = "PubulicSpace",
                            //    UserGender = "",
                            //    Email = "",
                            //};
                            //var addConferencRoomDeviceUser = new User
                            //{
                            //    UserName = "ConferencRoomDevice1",
                            //    UserAddress = "ConferencRoomDevice",
                            //    UserGender = "",
                            //    Email = "",
                            //};
                            //var addWebUser = new User
                            //{
                            //    UserName = "Web1",
                            //    UserAddress = "Web",
                            //    UserGender = "",
                            //    Email = "",
                            //};
                            //var addReseptionUser = new User
                            //{
                            //    UserName = "wwwwww",
                            //    UserAddress = "Reseption",
                            //    UserGender = "",
                            //    Email = "",
                            //};


                            var reselt = await userManager.CreateAsync(adduser, "Admin@01");
                            if (reselt.Succeeded)
                            {
                                await userManager.AddToRoleAsync(adduser, "Admin");
                            }
                            else
                            {
                                Console.Write(reselt.Errors);
                                transaction.Rollback();
                                return;
                            }
                            //    reselt = await userManager.CreateAsync(addRoomDeviceUser, "RoomDevice@01");
                            //    if (reselt.Succeeded)
                            //    {
                            //        await userManager.AddToRoleAsync(addRoomDeviceUser, "RoomDevice");
                            //    }
                            //    else
                            //    {
                            //        Console.Write(reselt.Errors);
                            //        transaction.Rollback();
                            //        return;
                            //    }
                            //    reselt = await userManager.CreateAsync(PubulicSpaceUser1, "PubulicSpace@01");
                            //    if (reselt.Succeeded)
                            //    {
                            //        await userManager.AddToRoleAsync(PubulicSpaceUser1, "PubulicSpace");
                            //    }
                            //    else
                            //    {
                            //        Console.Write(reselt.Errors);
                            //        transaction.Rollback();
                            //        return;
                            //    }
                            //    reselt = await userManager.CreateAsync(addConferencRoomDeviceUser, "ConferencRoomDevice@01");
                            //    if (reselt.Succeeded)
                            //    {
                            //        await userManager.AddToRoleAsync(addConferencRoomDeviceUser, "ConferencRoomDevice");
                            //    }
                            //    else
                            //    {
                            //        Console.Write(reselt.Errors);
                            //        transaction.Rollback();
                            //        return;
                            //    }
                            //    reselt = await userManager.CreateAsync(addWebUser, "Web@01");
                            //    if (reselt.Succeeded)
                            //    {
                            //        await userManager.AddToRoleAsync(addWebUser, "Web");
                            //    }
                            //    else
                            //    {
                            //        Console.Write(reselt.Errors);
                            //        transaction.Rollback();
                            //        return;
                            //    }
                            //    reselt = await userManager.CreateAsync(addReseptionUser, "Reseption@01");
                            //    if (reselt.Succeeded)
                            //    {
                            //        await userManager.AddToRoleAsync(addReseptionUser, "Reseption");
                            //    }
                            //    else
                            //    {
                            //        Console.Write(reselt.Errors);
                            //        transaction.Rollback();
                            //        return;
                            //    }
                            await context.SaveChangesAsync();
                        }
                        var user = userManager.FindByNameAsync("admin").Result;
                        if (user == null)
                        {
                            transaction.Rollback();
                            return;
                        }

                        //var RoomDeviceUser = userManager.FindByNameAsync("RoomDevice1").Result;
                        //if (RoomDeviceUser == null)
                        //{
                        //    transaction.Rollback();
                        //    return;
                        //}
                        //var PubulicSpaceUser = userManager.FindByNameAsync("PubulicSpace1").Result;
                        //if (PubulicSpaceUser == null)
                        //{
                        //    transaction.Rollback();
                        //    return;
                        //}
                        //var ConferencRoomDeviceUser = userManager.FindByNameAsync("ConferencRoomDevice1").Result;
                        //if (ConferencRoomDeviceUser == null)
                        //{
                        //    transaction.Rollback();
                        //    return;
                        //}
                        //var WebUser = userManager.FindByNameAsync("Web1").Result;
                        //if (WebUser == null)
                        //{
                        //    transaction.Rollback();
                        //    return;
                        //}
                        //var ReseptionUser = userManager.FindByNameAsync("wwwwww").Result;
                        //if (ReseptionUser == null)
                        //{
                        //    transaction.Rollback();
                        //    return;
                        //}
                        // Create Office
                        //if (!context.Offices.Any())
                        //{
                        //    var offices = new List<Office>
                        //            {
                        //                new Office { OfficeName = "Test", OfficeNameKana = "てすと",OfficeLocation="OIC",OfficeAddUserID=user.Id,OfficeAddTime=DateTime.Now,OfficeUpDateUserID=user.Id,OfficeUpDateTime=DateTime.Now,OfficeFlag=0},
                        //            };
                        //    foreach (var addoffice in offices)
                        //    {
                        //        context.Offices.Add(addoffice);
                        //    }
                        //    await context.SaveChangesAsync();
                        //}
                        //var office = context.Offices.FirstOrDefaultAsync().Result;
                        //if (office == null)
                        //{
                        //    transaction.Rollback();
                        //    return;
                        //}
                        //// Create Department
                        //if (!context.Departments.Any())
                        //{
                        //    var departments = new List<Department>
                        //            {
                        //                new Department { DepartmentName = "Test", DepartmentNameKana = "てすと", OfficeId=office.OfficeId,DepartmentAddUserID=user.Id,DepartmentAddTime=DateTime.Now,DepartmentUpDateUserID=user.Id,DepartmentUpDateTime=DateTime.Now},
                        //            };
                        //    foreach (var adddepartment in departments)
                        //    {
                        //        context.Departments.Add(adddepartment);
                        //    }
                        //    await context.SaveChangesAsync();
                        //}
                        //var department = context.Departments.FirstOrDefaultAsync().Result;
                        //if (department == null)
                        //{
                        //    transaction.Rollback();
                        //    return;
                        //}
                        //// Create section
                        //if (!context.Sections.Any())
                        //{
                        //    var sections = new List<Section>
                        //            {
                        //                new Section { SectionName = "Test", SectionNameKana = "てすと",SectionAddUserID=user.Id,SectionAddTime=DateTime.Now,SectionUpDateUserID=user.Id,SectionUpDateTime=DateTime.Now,DepartmentId=department.DepartmentId},
                        //            };
                        //    foreach (var addsection in sections)
                        //    {
                        //        context.Sections.Add(addsection);
                        //    }
                        //    await context.SaveChangesAsync();
                        //}


                        //// Create Room
                        //if (!context.Rooms.Any())
                        //{
                        //    var rooms = new List<Room>
                        //            {
                        //                new Room { RoomName = "会議室A",OfficeId=office.OfficeId,RoomAddUserID=user.Id,RommAddTime=DateTime.Now,RoompDateUserID=user.Id,RoomUpDateTime=DateTime.Now,RoomCapacity=8,RoomState=0,},
                        //                new Room { RoomName = "オフィス",OfficeId=office.OfficeId,RoomAddUserID=user.Id,RommAddTime=DateTime.Now,RoompDateUserID=user.Id,RoomUpDateTime=DateTime.Now,RoomCapacity=8,RoomState=0,},
                        //                new Room { RoomName = "public",OfficeId=office.OfficeId,RoomAddUserID=user.Id,RommAddTime=DateTime.Now,RoompDateUserID=user.Id,RoomUpDateTime=DateTime.Now,RoomCapacity=8,RoomState=0,},
                        //                new Room { RoomName = "受付",OfficeId=office.OfficeId,RoomAddUserID=user.Id,RommAddTime=DateTime.Now,RoompDateUserID=user.Id,RoomUpDateTime=DateTime.Now,RoomState=0,},

                        //            };
                        //    foreach (var addroom in rooms)
                        //    {
                        //        context.Rooms.Add(addroom);
                        //    }
                        //    await context.SaveChangesAsync();
                        //}
                        ////会議室Aの名前で取得
                        //var room = context.Rooms.FirstOrDefaultAsync(x => x.RoomName == "会議室A").Result;
                        //var room2 = context.Rooms.FirstOrDefaultAsync(x => x.RoomName == "オフィス").Result;
                        //var room3 = context.Rooms.FirstOrDefaultAsync(x => x.RoomName == "public").Result;
                        //var room4 = context.Rooms.FirstOrDefaultAsync(x => x.RoomName == "受付").Result;
                        //if (room == null || room2 == null || room3 == null || room4 == null)
                        //{
                        //    transaction.Rollback();
                        //    return;
                        //}
                        //// Create Device
                        //if (!context.Devices.Any())
                        //{
                        //    var devices = new List<Device>
                        //            {
                        //                new Device { DeviceName = "Test会議室",DeviceLocation="5-A",DeviceCategory=1,DeviceUserID=ConferencRoomDeviceUser.Id,RoomId=room.RoomId,DeviceAddUserID=user.Id,DeviceAddTime=DateTime.Now,DeviceUpdateUserID=user.Id,DeviceUpDateTime=DateTime.Now,DeviceFlag=0},
                        //                new Device { DeviceName = "Testオフィス",DeviceLocation="5-B",DeviceCategory=2,DeviceUserID=RoomDeviceUser.Id,RoomId=room2.RoomId,DeviceAddUserID=user.Id,DeviceAddTime=DateTime.Now,DeviceUpdateUserID=user.Id,DeviceUpDateTime=DateTime.Now,DeviceFlag=0},
                        //                new Device { DeviceName = "Testpublic",DeviceLocation="5-C",DeviceCategory=3,DeviceUserID=PubulicSpaceUser.Id,RoomId=room3.RoomId,DeviceAddUserID=user.Id,DeviceAddTime=DateTime.Now,DeviceUpdateUserID=user.Id,DeviceUpDateTime=DateTime.Now,DeviceFlag=0},
                        //                new Device { DeviceName = "Test受付",DeviceLocation="5-D",DeviceCategory=1,DeviceUserID=ReseptionUser.Id,RoomId=room4.RoomId,DeviceAddUserID=user.Id,DeviceAddTime=DateTime.Now,DeviceUpdateUserID=user.Id,DeviceUpDateTime=DateTime.Now,DeviceFlag=0},

                        //            };
                        //    foreach (var adddevice in devices)
                        //    {
                        //        context.Devices.Add(adddevice);
                        //    }
                        //    await context.SaveChangesAsync();
                        //}

                        //var device = context.Devices.FirstOrDefaultAsync(x => x.DeviceName == "Test会議室").Result;
                        //if (device == null)
                        //{
                        //    transaction.Rollback();
                        //    return;
                        //}


                        ////create NFC

                        //if (!context.Nfcs.Any())
                        //{
                        //    var nfc = new List<Nfc>
                        //            {
                        //        new Nfc{NfcUid="1",NfcAddUserID=user.Id,NfcAddTime=DateTime.Now,NfcUpdateUserID=user.Id,NfcUpdateTime=DateTime.Now,NfcState=0},
                        //        new Nfc{NfcUid="2",NfcAddUserID=user.Id,NfcAddTime=DateTime.Now,NfcUpdateUserID=user.Id,NfcUpdateTime=DateTime.Now,NfcState=0},
                        //        new Nfc{NfcUid="3",NfcAddUserID=user.Id,NfcAddTime=DateTime.Now,NfcUpdateUserID=user.Id,NfcUpdateTime=DateTime.Now,NfcState=0},
                        //        new Nfc{NfcUid="4",NfcAddUserID=user.Id,NfcAddTime=DateTime.Now,NfcUpdateUserID=user.Id,NfcUpdateTime=DateTime.Now,NfcState=0},

                        //            };
                        //    foreach (var addnfc in nfc)
                        //    {
                        //        context.Nfcs.Add(addnfc);
                        //    }
                        //    await context.SaveChangesAsync();
                        //}
                        //var nfc1 = context.Nfcs.FirstOrDefaultAsync(x => x.NfcUid == "1").Result;
                        //if (nfc1 == null)
                        //{
                        //    transaction.Rollback();
                        //    return;
                        //}
                        ////create NFCallotments

                        //if (!context.Nfcallotments.Any())
                        //{
                        //    var nfcallotments = new List<Nfcallotment>
                        //            {
                        //        new Nfcallotment{AllotmentTime=DateTime.Now,State=0,NfcId=nfc1.NfcId,ReservationId=null,NoReservationId=null,UserId=user.Id,AddUserId=user.Id,AddTime=DateTime.Now,UpdateUserId=user.Id,UpdateTime=DateTime.Now},
                        //        new Nfcallotment{AllotmentTime=DateTime.Now,State=0,NfcId=nfc1.NfcId,ReservationId=null,NoReservationId=null,UserId=user.Id,AddUserId=user.Id,AddTime=DateTime.Now,UpdateUserId=user.Id,UpdateTime=DateTime.Now},
                        //        new Nfcallotment{AllotmentTime=DateTime.Now,State=0,NfcId=nfc1.NfcId,ReservationId=null,NoReservationId=null,UserId=user.Id,AddUserId=user.Id,AddTime=DateTime.Now,UpdateUserId=user.Id,UpdateTime=DateTime.Now},
                        //        new Nfcallotment{AllotmentTime=DateTime.Now,State=0,NfcId=nfc1.NfcId,ReservationId=null,NoReservationId=null,UserId=user.Id,AddUserId=user.Id,AddTime=DateTime.Now,UpdateUserId=user.Id,UpdateTime=DateTime.Now},
                        //            };
                        //    foreach (var addnfcallotment in nfcallotments)
                        //    {
                        //        context.Nfcallotments.Add(addnfcallotment);
                        //    }
                        //    await context.SaveChangesAsync();

                        //}

                        //var nfcallotment = context.Nfcallotments.FirstOrDefaultAsync().Result;
                        //if (nfcallotment == null)
                        //{
                        //    transaction.Rollback();
                        //    return;
                        //}


                        //// Create Entrants

                        //if (!context.Entrants.Any())
                        //{
                        //    var entrants = new List<Entrants>
                        //            {
                        //            new Entrants { DeviceID = context.Devices.FirstOrDefaultAsync(x => x.DeviceName == "Test会議室").Result.DeviceId},
                        //            new Entrants { DeviceID = context.Devices.FirstOrDefaultAsync(x => x.DeviceName == "Testオフィス").Result.DeviceId},
                        //            new Entrants { DeviceID = context.Devices.FirstOrDefaultAsync(x => x.DeviceName == "Testpublic").Result.DeviceId},
                        //            new Entrants { DeviceID = context.Devices.FirstOrDefaultAsync(x => x.DeviceName == "Test受付").Result.DeviceId},

                        //            };
                        //    foreach (var addentrants in entrants)
                        //    {
                        //        context.Entrants.Add(addentrants);
                        //    }
                        //    await context.SaveChangesAsync();

                        //    var entrants1 = context.Entrants.Include(x => x.Nfcallotments).FirstOrDefaultAsync(x => x.Device.DeviceName == "Test会議室").Result;
                        //    if (entrants1 == null)
                        //    {
                        //        transaction.Rollback();
                        //        return;
                        //    }
                        //    entrants1.Nfcallotments.Add(nfcallotment);
                        //    await context.SaveChangesAsync();



                        //}

                        //// Create Reservation
                        //if (!context.Reservations.Any())
                        //{
                        //    var reservations = new List<Reservation>
                        //            {
                        //                new Reservation
                        //                    { ReservationName = "Test",
                        //                    ReservationNameKana = "てすと",
                        //                    ReservationNumberOfPersons = 1,
                        //                    ReservationRequirement = "Test",
                        //                    ReservationCompanyName = "Test",
                        //                    ReservationCompanyNameKana = "てすと",
                        //                    ReservationCompanyPosition = "Test",
                        //                    ReservationDate = DateTime.Now,
                        //                    ReservationEmail = "Test",
                        //                    ReservationPhoneNumber = "Test",
                        //                    ReservationState = 0,
                        //                    ReservationReception = 0,
                        //                    ReservationType = 0,
                        //                    ReservationQrcode = Encoding.UTF8.GetBytes("Test"),
                        //                    ReservationCode = "Test",
                        //                    Token = "Test",
                        //                    ReservationAddUserID = user.Id,
                        //                    ReservationAddTime = DateTime.Now,
                        //                    ReservationUpdateUserID = user.Id,
                        //                    ReservationUpDateTime = DateTime.Now
                        //                },
                        //                new Reservation
                        //                {
                        //                    ReservationName = "Test",
                        //                    ReservationNameKana = "てすと",
                        //                    ReservationNumberOfPersons = 1,
                        //                    ReservationRequirement = "Test",
                        //                    ReservationCompanyName = "Test",
                        //                    ReservationCompanyNameKana = "てすと",
                        //                    ReservationCompanyPosition = "Test",
                        //                    ReservationDate = DateTime.Now,
                        //                    ReservationEmail = "i400.hiro.2005@gmail.com",
                        //                    ReservationPhoneNumber = "Test",
                        //                    ReservationState = 0,
                        //                    ReservationReception = 0,
                        //                    ReservationType = 0,
                        //                    ReservationQrcode = Encoding.UTF8.GetBytes("Test"),
                        //                    ReservationCode = "Test",
                        //                    Token = "Test",
                        //                    ReservationAddUserID = user.Id,
                        //                    ReservationAddTime = DateTime.Now,
                        //                    ReservationUpdateUserID = user.Id,
                        //                    ReservationUpDateTime = DateTime.Now

                        //                }
                        //            };
                        //    foreach (var addreservation in reservations)
                        //    {
                        //        context.Reservations.Add(addreservation);
                        //    }
                        //    await context.SaveChangesAsync();
                        //}


                        transaction.Commit();





                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        var logger = services.GetRequiredService<ILogger<SeetData>>();
                        logger.LogError(ex, "An error occurred seeding the DB.");
                    }
                }
            }
        }
    }
}

