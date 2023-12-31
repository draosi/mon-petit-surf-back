﻿using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MonPetitSurf.Dtos;
using MonPetitSurf.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MonPetitSurf.Services
{
    public class UsersService
    {
        private readonly MonPetitSurfContext _context;
        public UsersService(MonPetitSurfContext context)
        {
            _context = context;
        }

        public async Task<bool> registerUser(UserRegistrationDto registrationDto)
        {
            if (isUserNameAvailable(registrationDto.Username))
            {
                return false;
            }

            string saltedPassword = BCrypt.Net.BCrypt.GenerateSalt(12);
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registrationDto.Password, saltedPassword);

            var user = new Users
            {
                Username = registrationDto.Username,
                Password = hashedPassword,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }

        private bool isUserNameAvailable(string userName)
        {
            return _context.Users.Any(e => e.Username == userName);
        }

        public async Task<Users> getUserByUsername(string username)
        {
            // FirstOrDefaultAsync permet de renvoyer null plutot qu'un erreur si la condition n'est pas rempli
            return await _context.Users.FirstOrDefaultAsync(e => e.Username == username);
        }

        public string generateJwtToken(int id)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secrets.JWT_SECRET));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                // Ajoute l'identifiant de l'utilisateur comme revendication.
                new Claim(JwtRegisteredClaimNames.NameId, id.ToString())
            };

            var token = new JwtSecurityToken(
                    issuer: Secrets.Issuer,
                    audience: Secrets.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddHours(2),
                    signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<Users> getUserById(int id)
        {
            return (await _context.Users.FindAsync(id));
        }

        public async Task<bool> userExist(int id)
        {
            return await _context.Users.AnyAsync(e => e.Id == id);
        }

        public async Task<bool> deleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return false;
            }

            var userFavorites = await _context.UsersRegisterSpots
                .Where(e => e.UserId == user.Id)
                .ToListAsync();

            foreach (var item in userFavorites)
            {
                _context.UsersRegisterSpots.Remove(item);
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> updateUser(Users user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<UsersRegisterSpots>> getUserFavorites(int id)
        {
            var favorites = await _context.UsersRegisterSpots
                .Where(e => e.UserId == id)
                .ToListAsync();

            return favorites;
        }

        public async Task addFavorite(int userId, int spotId)
        {
            var favorite = new UsersRegisterSpots
            {
                UserId = userId,
                SpotId = spotId,
                CreatedAt = DateTime.Now,
            };

            _context.UsersRegisterSpots.Add(favorite);
            await _context.SaveChangesAsync();
        }


        public async Task deleteFavorite(int userId, int spotId)
        {
            var favorite = await _context.UsersRegisterSpots.SingleOrDefaultAsync(e => e.UserId == userId && e.SpotId == spotId);

            if (favorite != null)
            {
                _context.UsersRegisterSpots.Remove(favorite);
                await _context.SaveChangesAsync();
            }
        }
    }
}
