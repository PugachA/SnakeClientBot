using RestSharp;
using SnakeClient.DTO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SnakeClient.Models
{
    public class SnakeAPIClient
    {
        private readonly RestClient _restClient;
        private readonly string token;
        public string ErrorMessage { get; private set; }

        public SnakeAPIClient(Uri uri, string token)
        {
            this.token = token;
            this._restClient = new RestClient(uri);
        }

        public async Task<SnakeAPIResponse<GameStateDto>> GetGameState()
        {
            try
            {
                var request = new RestRequest("/api/Player/gameboard");
                request.AddParameter(nameof(token), token);

                var response = await _restClient.ExecuteGetTaskAsync<GameStateDto>(request);

                if (!response.IsSuccessful)
                    return new SnakeAPIResponse<GameStateDto> { ErrorMessage = $"Запрос неудачен.", Data = null };

                return new SnakeAPIResponse<GameStateDto> {ErrorMessage = null, Data = response.Data, IsSuccess = true };
            }
            catch(Exception ex)
            {
                return new SnakeAPIResponse<GameStateDto> { ErrorMessage = ex.ToString(), Data = null };
            }
        }

        public async Task<SnakeAPIResponse<NameResponseDto>> GetNameResponse()
        {
            try
            {
                var request = new RestRequest("/api/Player/name");
                request.AddParameter(nameof(token), token);

                var response = await _restClient.ExecuteGetTaskAsync<NameResponseDto>(request);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return new SnakeAPIResponse<NameResponseDto> { ErrorMessage = "Некорректный токен", Data = null };

                if (!response.IsSuccessful)
                    return new SnakeAPIResponse<NameResponseDto> { ErrorMessage = "Запрос неудачен", Data = null };

                return new SnakeAPIResponse<NameResponseDto> { ErrorMessage = null, Data = response.Data, IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new SnakeAPIResponse<NameResponseDto> { ErrorMessage = ex.ToString(), Data = null };
            }
        }

        public async Task<SnakeAPIResponse<object>> PostDirection(Direction direction)
        {
            try
            {
                var request = new RestRequest("/api/Player/direction");
                request.Method = Method.POST;
                DirectionDto directionDto = new DirectionDto { Direction = direction, Token = this.token };
                request.AddParameter("application/json", JsonSerializer.Serialize(directionDto), ParameterType.RequestBody);

                var response = await _restClient.ExecutePostTaskAsync(request);

                if(response.StatusCode == HttpStatusCode.Unauthorized)
                    return new SnakeAPIResponse<object> { ErrorMessage = "Некорректный токен", Data = null };

                if (!response.IsSuccessful)
                    return new SnakeAPIResponse<object> { ErrorMessage = "Запрос неудачен", Data = null };

                return new SnakeAPIResponse<object> { ErrorMessage = null, Data = null, IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new SnakeAPIResponse<object> { ErrorMessage = ex.ToString(), Data = null};
            }
        }
    }
}
