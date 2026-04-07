using Common;
using Contracts.Repository.Base;
using Contracts.Service.Base;
using Model.FuncResult;
using Model.Request.Base;

namespace Service.Base
{
    public abstract class CRUDServiceWithCache<T> : BaseService, ICRUDService<T>
    {
        private ICreateService<T> _createService;
        private IReadService<T> _readService;
        private IUpdateService<T> _updateService;
        private IDeleteService<T> _deleteService;
        protected ICRUDRepository<T> _repositoryBase;

        protected string _keyPrefix;
        protected string _itemKeyField;
        protected string _itemKeyFieldOption;

        public CRUDServiceWithCache(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.ConfigKey();
        }
        protected abstract void ConfigKey();


        public async Task<bool> DeleteAsync(int id)
        {
            _deleteService = _deleteService ??= new DeleteService<T>(_serviceProvider, _repositoryBase);
            var result = await _deleteService.DeleteAsync(id);
            try
            {
                if (id.ToString() != string.Empty)
                {
                    await _serviceWrapper.Cache.RemoveDataAsync($"{_keyPrefix}:{id.ToString()}");
                }
                return result;
            }
            catch (System.Exception ex)
            {
                ex.SaveLog();
                return result;
            }
        }

        public async Task<int> InsertAsync(T obj)
        {
            _createService = _createService ??= new CreateService<T>(_serviceProvider, _repositoryBase);
            var id = await _createService.InsertAsync(obj);
            try
            {
                if (id.ToString() != string.Empty)
                {
                    obj.SetPropValue(_itemKeyField, id);
                    await _serviceWrapper.Cache.SetDataAsync($"{_keyPrefix}:{id.ToString()}", obj, null);
                    if (_itemKeyFieldOption.ConvertToString() != "")
                    {
                        var itemFieldOptionValue = obj.GetPropValue(_itemKeyFieldOption);
                        await _serviceWrapper.Cache.SetDataAsync($"option_{_keyPrefix}:{itemFieldOptionValue.ConvertToString()}", id.ToString(), null);
                    }
                }
                return id;
            }
            catch (System.Exception ex)
            {
                ex.SaveLog();
                return id;
            }
        }

        public async Task<IEnumerable<T>> SelectAllAsync()
        {
            try
            {
                var list = await _serviceWrapper.Cache.GetListDataAsync<T>($"{_keyPrefix}*");
                if (list == null || list.Count() == 0)
                {
                    _readService = _readService ??= new ReadService<T>(_serviceProvider, _repositoryBase);
                    list = await _readService.SelectAllAsync();
                    await _serviceWrapper.Cache.SetListDataAsync<T>($"{_keyPrefix}", _itemKeyField, list, null);
                    if (_itemKeyFieldOption.ConvertToString() != "")
                    {
                        var dictionary = list
                        .Select(x => new { value = x.GetPropValue(_itemKeyField).ConvertToString(), key = x.GetPropValue(_itemKeyFieldOption).ConvertToString() })
                        .GroupBy(item => item.key)
                        .ToDictionary(
                            group => group.Key,
                            group => group.Last().value
                        );
                        var cachedDictionaryKeys = (await _serviceWrapper.Cache.GetKeysAsync($"option_{_keyPrefix}*"))
                   .Select(x => x.Replace($"option_{_keyPrefix}:", "")).ToList();
                        var filteredDictionary = dictionary
                        .Where(kv => !cachedDictionaryKeys.Contains(kv.Key))
                        .ToDictionary(kv => kv.Key, kv => kv.Value);

                        await _serviceWrapper.Cache.SetDictionaryDataAsync($"option_{_keyPrefix}", filteredDictionary, null);
                    }
                }
                return list;
            }
            catch (System.Exception ex)
            {
                ex.SaveLog();
                _readService = _readService ??= new ReadService<T>(_serviceProvider, _repositoryBase);
                var list = await _readService.SelectAllAsync();
                await _serviceWrapper.Cache.SetListDataAsync<T>($"{_keyPrefix}", _itemKeyField, list, null);
                if (_itemKeyFieldOption.ConvertToString() != "")
                {
                    // await _serviceWrapper.Cache.SetListDataAsync<T>($"option_{_keyPrefix}", _itemKeyFieldOption, list, null);
                    var dictionary = list
                        .Select(x => new { value = x.GetPropValue(_itemKeyField).ConvertToString(), key = x.GetPropValue(_itemKeyFieldOption).ConvertToString() })
                        .GroupBy(item => item.key)
                        .ToDictionary(
                            group => group.Key,
                            group => group.Last().value
                        );
                    var cachedDictionaryKeys = (await _serviceWrapper.Cache.GetKeysAsync($"option_{_keyPrefix}*"))
               .Select(x => x.Replace($"option_{_keyPrefix}:", "")).ToList();
                    var filteredDictionary = dictionary
                    .Where(kv => !cachedDictionaryKeys.Contains(kv.Key))
                    .ToDictionary(kv => kv.Key, kv => kv.Value);
                    await _serviceWrapper.Cache.SetDictionaryDataAsync($"option_{_keyPrefix}", filteredDictionary, null);
                }
                return list;
            }

        }

        public async Task<T> SelectByIdAsync(int id)
        {
            try
            {
                return await _serviceWrapper.Cache.GetDataAsync<T>($"{_keyPrefix}:{id.ToString()}");
            }
            catch (System.Exception ex)
            {
                ex.SaveLog();
                _readService = _readService ??= new ReadService<T>(_serviceProvider, _repositoryBase);
                return await _readService.SelectByIdAsync(id);
            }
        }
        public async Task<T> SelectByOptionKeyAsync(string key)
        {
            try
            {
                var id = await _serviceWrapper.Cache.GetDataAsync<string>($"option_{_keyPrefix}:{key}");
                if (id.ConvertToInt() > 0)
                {
                    return await this.SelectByIdAsync(id.ConvertToInt());
                }
                return default(T);
            }
            catch (System.Exception ex)
            {
                ex.SaveLog();
                return default(T);
                // _readService = _readService ??= new ReadService<T>(_serviceProvider, _repositoryBase);
                // return await _readService.SelectByIdAsync(id);
            }
        }

        public async Task<bool> UpdateAsync(T obj)
        {
            _updateService = _updateService ?? new UpdateService<T>(_serviceProvider, _repositoryBase);
            var result = await _updateService.UpdateAsync(obj);
            try
            {
                var id = obj.GetPropValue(_itemKeyField)?.ToString() ?? "";
                if (id != string.Empty)
                {
                    await _serviceWrapper.Cache.SetDataAsync<T>($"{_keyPrefix}:{id.ToString()}", obj, null);
                    if (_itemKeyFieldOption.ConvertToString() != "")
                    {
                        var itemFieldOptionValue = obj.GetPropValue(_itemKeyFieldOption);
                        await _serviceWrapper.Cache.SetDataAsync($"option_{_keyPrefix}:{itemFieldOptionValue.ConvertToString()}", id, null);
                    }
                }
                return result;
            }
            catch (System.Exception ex)
            {
                ex.SaveLog();
                return result;
            }

        }

        public Task<PagingResult<IEnumerable<T>>> SelectAsync(PagingRequest pagingRequest)
        {
            _readService = _readService ??= new ReadService<T>(_serviceProvider, _repositoryBase);
            return _readService.SelectAsync(pagingRequest);
        }

        public async Task<bool> ClearCacheAsync()
        {
            await _serviceWrapper.Cache.RemoveDataByPatternAsync($"{_keyPrefix}*");
            if (_itemKeyFieldOption.ConvertToString() != "")
            {
                await _serviceWrapper.Cache.RemoveDataByPatternAsync($"option_{_keyPrefix}*");
            }
            return true;
        }

        public async Task<IEnumerable<T>> ClearCacheThenSelectAllAsync()
        {
            await this.ClearCacheAsync();
            return await this.SelectAllAsync();
            return null;
        }
        public async Task<bool> EnsureCachedDateUpdatedByLastUpdatTimeAsync()
        {
            try
            {
                var cachedData = (await _serviceWrapper.Cache.GetListDataAsync<T>($"{_keyPrefix}*")).ToList();
                var cachedDataKeys = await _serviceWrapper.Cache.GetKeysAsync($"{_keyPrefix}*");
                cachedDataKeys = cachedDataKeys.Select(x => x.Replace($"{_keyPrefix}:", "")).ToList();
                _readService = _readService ??= new ReadService<T>(_serviceProvider, _repositoryBase);
                var dbValues = await _readService.SelectAllAsync();
                var dbKeys = dbValues.Select(x => x.GetPropValue(_itemKeyField).ConvertToString()).ToList();
                //delete
                var deleteKeys = cachedDataKeys.Where(x => !dbKeys.Contains(x)).ToList();
                await _serviceWrapper.Cache.RemoveDataAsync(deleteKeys.Select(x => $"{_keyPrefix}:{x}").ToList());
                //insert
                var insertKeys = dbKeys.Where(x => !cachedDataKeys.Contains(x)).ToList();
                var insertDatas = dbValues.Where(x => insertKeys.Contains(x.GetPropValue(_itemKeyField).ConvertToString())).ToList();
                await _serviceWrapper.Cache.SetListDataAsync<T>($"{_keyPrefix}", _itemKeyField, insertDatas, null);
                //update
                var updateDatas = (from a in dbKeys
                                   join b in cachedDataKeys on a equals b
                                   join c in dbValues on a equals c.GetPropValue(_itemKeyField).ConvertToString()
                                   join d in cachedData on b equals d.GetPropValue(_itemKeyField).ConvertToString()
                                   where c.GetPropValue("last_modified_times").ToString() != d.GetPropValue("last_modified_times").ToString()
                                   select c).ToList();
                await _serviceWrapper.Cache.SetListDataAsync<T>($"{_keyPrefix}", _itemKeyField, updateDatas, null);
                if (_itemKeyFieldOption.ConvertToString() != "")
                {
                    cachedData = (await _serviceWrapper.Cache.GetListDataAsync<T>($"{_keyPrefix}*")).ToList();
                    var dictionary = cachedData
                        .Select(x => new { value = x.GetPropValue(_itemKeyField).ConvertToString(), key = x.GetPropValue(_itemKeyFieldOption).ConvertToString() })
                       .GroupBy(item => item.key)
                        .ToDictionary(
                            group => group.Key,
                            group => group.Last().value
                        );
                    var cachedDictionaryKeys = (await _serviceWrapper.Cache.GetKeysAsync($"option_{_keyPrefix}*"))
                    .Select(x => x.Replace($"option_{_keyPrefix}:", "")).ToList();
                    var filteredDictionary = dictionary
                    .Where(kv => !cachedDictionaryKeys.Contains(kv.Key))
                    .ToDictionary(kv => kv.Key, kv => kv.Value);
                    await _serviceWrapper.Cache.SetDictionaryDataAsync($"option_{_keyPrefix}", filteredDictionary, null);
                }
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }
    }
}

