using KitAplication.Data;
using KitAplication.Interface;
using KitAplication.Models;
using System.Xml;

namespace KitAplication.Services
{
    public class SystemService : ISystemService
    {
        private readonly IRepository<SystemEntity> _systemRepository;
        private readonly IRepository<MessageEntity> _messageRepository;

        public SystemService(IRepository<SystemEntity> systemRepository, IRepository<MessageEntity> messageRepository)
        {
            _systemRepository = systemRepository;
            _messageRepository = messageRepository;
        }



        /// <summary>
        ///  Creates a new system in the database based on the provided SystemModel object.
        /// </summary>
        /// <param name="model">The SystemModel object containing the data for the new system.</param>
        /// <returns>The newly created SystemModel object.</returns>
        public async Task<SystemModel> CreateSystemAsync(SystemModel model)
        {
            var systementity = new SystemEntity
            {
                SystemName = model.SystemName,
                Model = model.Model,
                Prefix = model.Prefix ??= "",
                SystemContent = model.SystemContent,
            };
            var entity = await  _systemRepository.AddAsync(systementity);
            var createdSystem = new SystemModel()
            {
                Id = entity.Id,
                SystemName = entity.SystemName,
                SystemContent = entity.SystemContent,
                IsActive = entity.IsActive,
                RoleName = entity.RoleName,
                Model = entity.Model,
                Prefix = entity.Prefix
            };
            
            return createdSystem;
        }

        /// <summary>
        /// Retrieves a SystemModel object from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the system to retrieve</param>
        /// <returns>The SystemModel object with the specified ID, or null if it does not exist.</returns>
        public async Task<SystemModel> GetSystemByIdAsync(int id)
        {
            var system = await _systemRepository.GetByIdAsync(id);

            if (system == null)
            {
                return null;
            }

            return new SystemModel
            {
                Id = system.Id,
                SystemName = system.SystemName,
                RoleName = system.RoleName,
                Model = system.Model,
                Prefix = system.Prefix,
                SystemContent = system.SystemContent,
                IsActive = system.IsActive
            };
        }

        /// <summary>
        /// Updates a SystemModel object asynchronously in the _systemRepository if it exist
        /// </summary>
        /// <param name="model">The SystemModel object to update</param>
        /// <returns>A Task containing the updated SystemModel object</returns>
        public async Task<SystemModel> UpdateSystemAsync(SystemModel model)
        {
            var system = await _systemRepository.GetByIdAsync(model.Id);

            if (system != null)
            {
                system.SystemName = model.SystemName;
                system.RoleName = model.RoleName;
                system.Model = model.Model;
                system.Prefix = model.Prefix??="";
                system.SystemContent = model.SystemContent;

               await _systemRepository.UpdateAsync(system);
            }
            return model;
        }
        /// <summary>
        /// Deletes a system from the database based on the specified ID.
        /// </summary>
        /// <param name="id">The ID of the system to delete</param>
        /// <returns>True if the system was successfully deleted, false otherwise</returns>
        public async Task<bool> DeleteSystem(int id)
        {
            var system = await _systemRepository.GetByIdAsync(id);

            if (system != null)
            {
               await  _systemRepository.DeleteAsync(system);
                return true;

            }return false;
        }

        /// <summary>
        /// Retrieves the active system from the database.
        /// </summary>
        /// <returns>The SystemModel object representing the active system, or null if there is no active system.</returns>
        public async Task<SystemModel> GetActiveSystem()
        {
            var entrioty = await _systemRepository.GetAsync(x => x.IsActive == true);
            
            var activeSystem = entrioty.FirstOrDefault();
            if (activeSystem != null)
            {
                return new SystemModel
                {
                    Id = activeSystem.Id,
                    SystemName = activeSystem.SystemName,
                    RoleName = activeSystem.RoleName,
                    Model = activeSystem.Model,
                    Prefix = activeSystem.Prefix,
                    SystemContent = activeSystem.SystemContent,
                    IsActive = activeSystem.IsActive
                };
            }
            return null;
        }
        /// <summary>
        /// Activates the system with the specified ID and deactivates all other systems
        /// </summary>
        /// <param name="id">The ID of the system to activate</param>
        /// <returns>True if the system was successfully activated, false otherwise</returns>
        public async Task<bool> ActivateSystem(int id)
        {
            var active = await _systemRepository.GetByIdAsync(id);
            if (active != null)
            {
                //sett all the others to false
                var aktivesystems = await _systemRepository.GetAsync(x => x.IsActive);
                foreach (var system in aktivesystems)
                {
                    system.IsActive = false;
                    await _systemRepository.UpdateAsync(system);
                }

                //set this to active
                active.IsActive = true;
                await _systemRepository.UpdateAsync(active);

               
                return true;
            }
            else return false;
        }
        public async Task<bool> DeactivateSystem(int id)
        {
            var system = await _systemRepository.GetByIdAsync(id);
            if (system != null) {
                system.IsActive = false;
                await _systemRepository.UpdateAsync(system);
                return true;
              }
            return false;
        }
        /// <summary>
        /// Retrieves all systems from the _systemRepository.
        /// </summary>
        /// <returns>A list of SystemModel objects representing all the systems in the database.</returns>
        public async Task<List<SystemModel>> GetAllAsync()
        {
            var systems = await _systemRepository.GetAllAsync();
            return systems.Select(m => new SystemModel
            {
                Id = m.Id,
                SystemName=m.SystemName,
                RoleName= m.RoleName,
                Model = m.Model,
                Prefix = m.Prefix,
                SystemContent = m.SystemContent,
                IsActive = m.IsActive
            }).ToList();
        }

        /// <summary>
        /// Deletes a system and all associated messages from the database based on the specified ID.
        /// </summary>
        /// <param name="id">The ID of the system to delete.</param>
        /// <returns>True if the system and its associated messages were successfully deleted, false otherwise</returns>
        public async Task<bool> DeleteSystemAndMessagesAsync(int id)
        {
            var system = await _systemRepository.GetByIdAsync(id);

            if (system != null)
            {
                var messages = await _messageRepository.GetAsync(x => x.SystemId == system.Id);
                foreach(var m in messages)
                {
                    await _messageRepository.DeleteAsync(m);
                }

                await _systemRepository.DeleteAsync(system);
                return true;

            }
            return false;
        }
    }
}
