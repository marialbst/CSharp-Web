namespace SocialNetwork
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using SocialNetwork.Data;
    using SocialNetwork.Data.Logic;

    class Program
    {
        private static Random rand = new Random();

        static void Main(string[] args)
        {
            using (var db = new SocialNetworkDbContext())
            {
                db.Database.Migrate();

                //SeedUsers(db);
                //SeedAlbumsAndPictures(db);
                //SeedTags(db);
                //PrintUsersWithFriends(db);
                //PrintUsersWithMoreThan5Frinds(db);
                //PrintAlbumsWithTotalPictures(db);
                //PrintPicturesInMoreThan2albums(db);
                //PrintAlbumsByUser(db);

                PrintAlbumsByGivenTag(db);
                PrintUsersWithMoreThanThreeAlbums(db);
            }
        }

        private static void SeedUsers(SocialNetworkDbContext db)
        {
            const int totalUsers = 50;
            int biggestId = db.Users
                .OrderByDescending(u => u.Id)
                .Select(u => u.Id)
                .FirstOrDefault() + 1;
            List<User> allUsers = new List<User>();

            for (int i = biggestId; i < biggestId + totalUsers; i++)
            {
                var user = new User
                {
                    Username = $"user {i}",
                    Password = $"aB{i}defG#!",
                    Email = $"test{i}@abv.bg",
                    RegisteredOn = DateTime.Now.AddDays(10 - 100 * i),
                    LastTimeLoggedIn = DateTime.Now.AddDays(i),
                    Age = 10 * i / 5
                };

                allUsers.Add(user);
                db.Users.Add(user);
            }

            db.SaveChanges();

            var userIds = allUsers.Select(u => u.Id).ToList();

            for (int i = 0; i < userIds.Count(); i++)
            {
                var currentUserId = userIds[i];
                var totalFriends = rand.Next(5,11);

                for (int j = 0; j < totalFriends; j++)
                {
                    var friendId = userIds[rand.Next(0, userIds.Count())];
                    var validFriendship = true;

                    if (friendId == currentUserId)
                    {
                        validFriendship = false;
                    }

                    bool friendshipExists = db.Friendships
                        .Any(f => (f.FromUserId == currentUserId && f.ToUserId == friendId) || 
                            (f.FromUserId == friendId && f.ToUserId == currentUserId));

                    if (friendshipExists)
                    {
                        validFriendship = false;
                    }

                    if (!validFriendship)
                    {
                        j--;
                        continue;
                    }

                    db.Friendships.Add(new Friendship
                    {
                        FromUserId = currentUserId,
                        ToUserId = friendId
                    });
                    db.SaveChanges();
                }               
            }
        }

        private static void SeedAlbumsAndPictures(SocialNetworkDbContext db)
        {
            const int totalAlbums = 100;
            const int totalPictures = 500;

            var totalUserIds = db.Users
                .Select(u => u.Id)
                .ToList();

            var biggestAlbumId = db.Albums
                .OrderByDescending(a => a.Id)
                .Select(a => a.Id)
                .FirstOrDefault() + 1; ;

            var albums = new List<Album>();

            for (int i = biggestAlbumId; i < biggestAlbumId + totalAlbums; i++)
            {
                var album = new Album
                {
                    Name = $"Album {i}",
                    Background = $"Color {i}",
                    IsPublic = i % 3 == 2,
                    OwnerId = totalUserIds[rand.Next(0, totalUserIds.Count)]
                };
                db.Albums.Add(album);
                albums.Add(album);
            }

            db.SaveChanges();

            var biggestPictureId = db.Pictures
                .OrderByDescending(p => p.Id)
                .Select(p => p.Id)
                .FirstOrDefault() + 1; ;

            var pictures = new List<Picture>();

            for (int i = biggestPictureId; i < biggestPictureId + totalPictures; i++)
            {
                var picture = new Picture
                {
                    Title = $"picture {i}",
                    Caption = $"caption {i}",
                    Path = $"http://path{i}.com"
                };
                db.Pictures.Add(picture);
                pictures.Add(picture);
            }

            db.SaveChanges();

            var totalAlbumIds = albums.Select(a => a.Id).ToList();
            var totalPictureIds = pictures.Select(p => p.Id).ToList();

            for (int i = 0; i < totalPictureIds.Count; i++)
            {
                var numberOfAlbumes = rand.Next(0, 20);
                var pictureId = totalPictureIds[i];

                for (int j = 0; j < numberOfAlbumes; j++)
                {
                    var albumId = totalAlbumIds[rand.Next(0, totalAlbumIds.Count)];

                    if (!db.AlbumPictures.Any(p => p.PictureId == pictureId && p.AlbumId == albumId))
                    {
                        db.AlbumPictures.Add(new AlbumPicture { AlbumId = albumId, PictureId = pictureId });
                    }
                    else
                    {
                        j--;
                    }
                    db.SaveChanges();
                }
            }
        }

        private static void SeedTags(SocialNetworkDbContext db)
        {
            int totalTags = db.Albums.Count() * 20;
            var tags = new List<Tag>();

            for (int i = 0; i < totalTags; i++)
            {
                var tag = new Tag
                {
                    Name = TagTransformer.TransformTag($"tag {i}")
                };
                
                db.Tags.Add(tag);
                tags.Add(tag);
            }

            db.SaveChanges();

            var albumIds = db.Albums.Select(a => a.Id).ToList();

            foreach (var tag in tags)
            {
                var totalAlbums = rand.Next(0, 20);

                for (int i = 0; i < totalAlbums; i++)
                {
                    var albumid = albumIds[rand.Next(0, albumIds.Count)];
                    var isTagExisting = db.Albums.Any(a => a.Id == albumid && a.Tags.Any(t => t.TagId == tag.Id));

                    if (isTagExisting)
                    {
                        i--;
                        continue;
                    }

                    tag.Albums.Add(new AlbumTag { AlbumId = albumid});
                    db.SaveChanges();
                }
            }            
        }


        private static void PrintUsersWithFriends(SocialNetworkDbContext db)
        {
            var users = db.Users
                .Select(u => new
                {
                    u.Username,
                    Friends = u.FromFriends.Count + u.ToFriends.Count,
                    IsActive = !u.IsDeleted ? "Active" : "Inactive"
                })
                .OrderByDescending(u => u.Friends)
                .ThenBy(u => u.Username);

            foreach (var user in users)
            {
                Console.WriteLine($"{user.Username} - {user.Friends} friends; {user.IsActive}");
            }
        }

        private static void PrintUsersWithMoreThan5Frinds(SocialNetworkDbContext db)
        {
            var users = db.Users
                .Where(u => !u.IsDeleted && (u.ToFriends.Count + u.FromFriends.Count) > 5)
                .OrderBy(u => u.RegisteredOn)
                .ThenByDescending(u => u.ToFriends.Count + u.FromFriends.Count)
                .Select(u => new
                {
                    u.Username,
                    Friends = u.FromFriends.Count + u.ToFriends.Count,
                    PeriodOfActive = DateTime.Now.Subtract(u.RegisteredOn.Value).Days
                });

            foreach (var user in users)
            {
                Console.WriteLine($"{user.Username} - {user.Friends} friends - {user.PeriodOfActive} days");
            }
        }

        private static void PrintAlbumsWithTotalPictures(SocialNetworkDbContext db)
        {
            var result = db.Albums
                .Select(a => new
                {
                    Title = a.Name,
                    Owner = a.Owner.Username,
                    Pictures = a.Pictures.Count
                })
                .OrderByDescending(a => a.Pictures)
                .ThenBy(a => a.Owner);

            foreach (var album in result)
            {
                Console.WriteLine($"{album.Title} - owner: {album.Owner}, {album.Pictures} pictures");
            }
        }

        private static void PrintPicturesInMoreThan2albums(SocialNetworkDbContext db)
        {
            var pictures = db.Pictures
                .Where(p => p.Albums.Count >= 2)
                .OrderByDescending(p => p.Albums.Count)
                .ThenBy(p => p.Title)
                .Select(p => new
                {
                    p.Title,
                    Albums = p.Albums.Select(a => a.Album.Name),
                    Owners = p.Albums.Select(a => a.Album.Owner.Username)
                });

            foreach (var picture in pictures)
            {
                Console.WriteLine($"{picture.Title} - Albums: {string.Join(", ", picture.Albums)}; Owners: {string.Join(", ", picture.Owners)}");
            }
        }

        private static void PrintAlbumsByUser(SocialNetworkDbContext db)
        {
            var userId = 1;

            var albums = db.Albums
                .Where(a => a.OwnerId == userId)
                .OrderBy(a => a.Name)
                .Select(a => new
                {
                    Owner = a.Owner.Username,
                    a.IsPublic,
                    a.Name,
                    Images = a.Pictures.Select(p => new
                    {
                        Name = p.Picture.Title,
                        Path = p.Picture.Path
                    })
                });

            
            foreach (var album in albums)
            {
                Console.WriteLine(album.Owner);

                Console.WriteLine($"--Album {album.Name}");

                if (album.IsPublic)
                {
                    foreach (var pic in album.Images)
                    {
                        Console.WriteLine($"#######{pic.Name} - {pic.Path}");
                    }
                }
                else
                {
                    Console.WriteLine("Private content!");
                }
            }
        }

        private static void PrintAlbumsByGivenTag(SocialNetworkDbContext db)
        {
            var tag = "#tag4030";

            var albums = db.Albums
                .Where(a => a.Tags.Any(t => t.Tag.Name == tag))
                .OrderByDescending(a => a.Tags.Count)
                .ThenBy(a => a.Name)
                .Select(a => new
                {
                    a.Name,
                    Owner = a.Owner.Username
                });

            foreach (var album in albums)
            {
                Console.WriteLine($"{album.Name} owned by {album.Owner}");
            }
        }

        private static void PrintUsersWithMoreThanThreeAlbums(SocialNetworkDbContext db)
        {
            var users = db.Users
                .Where(u => u.Albums.Any(a => a.Tags.Count > 3))
                .Select(u => new
                {
                    u.Username,
                    Albums = u.Albums
                        .Where(a => a.Tags.Count > 3)
                        .Select(a => new
                        {
                            a.Name,
                            Tags = a.Tags.Select(t => t.Tag.Name)
                        })
                })
                .OrderByDescending(u => u.Albums.Count())
                .ThenByDescending(u => u.Albums.Sum(a => a.Tags.Count()))
                .ThenBy(u => u.Username);

            foreach (var user in users)
            {
                Console.WriteLine($"{user.Username}");

                foreach (var album in user.Albums)
                {
                    Console.WriteLine($"{album.Name} - [{string.Join(", ", album.Tags)}]");
                }
            }
        }
    }
}
