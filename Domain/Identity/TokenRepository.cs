using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Identity
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IdentityDBContext _dbContext;
        public TokenRepository(IdentityDBContext dBContext)
        {
            _dbContext = dBContext;
        }
        public async Task<TokenRefresh> Add(TokenRefresh tokenRefresh)
        {
            _dbContext.TokenRefreshs.Add(tokenRefresh);
            await _dbContext.SaveChangesAsync();
            return tokenRefresh;
        }

        public async Task<TokenRefresh?> GetByUserID(Guid userID)
        {
            var tokenRefresh = await _dbContext.TokenRefreshs.Where(t=>t.UserID == userID).FirstOrDefaultAsync();
            return tokenRefresh;
        }

        public async Task<TokenRefresh> Update(TokenRefresh tokenRefresh)
        {
            _dbContext.TokenRefreshs.Update(tokenRefresh);
            await _dbContext.SaveChangesAsync();
            return tokenRefresh;
        }

        public async Task<bool> Delete(Guid userID)
        {
            var tokenRefresh = await _dbContext.TokenRefreshs.Where(t => t.UserID == userID).FirstOrDefaultAsync();
            if (tokenRefresh == null)
            {
                throw new Exception("token found");
            }

            _dbContext.TokenRefreshs.Remove(tokenRefresh);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }

    public interface ITokenRepository
    {
        Task<TokenRefresh> Add(TokenRefresh tokenRefresh);
        Task<TokenRefresh> Update(TokenRefresh tokenRefresh);
        Task<TokenRefresh?> GetByUserID(Guid userID);

        Task<bool> Delete(Guid userID);
    }
}
