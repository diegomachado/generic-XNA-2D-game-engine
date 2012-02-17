using System;
using System.Threading;

using Lidgren.Network;

namespace XnaGameServer
{
	class Program
	{
		static void Main(string[] args)
		{
            NetPeerConfiguration config = new NetPeerConfiguration("projetofinal");
			config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
			config.Port = 666;

			// Cria e inicia o Servidor
			NetServer server = new NetServer(config);
			server.Start();

			// Agenda o envio de atualizações
            double nextSendUpdates = NetTime.Now;

			while (!Console.KeyAvailable || Console.ReadKey().Key != ConsoleKey.Escape)
			{
                // Mensagem de algum cliente para o Servidor
                NetIncomingMessage im ;

                while ((im = server.ReadMessage()) != null)
				{
                    switch (im.MessageType)
					{
						// Faz o Hand-Shake inicial com o cliente
                        case NetIncomingMessageType.DiscoveryRequest:
                            server.SendDiscoveryResponse(null, im.SenderEndpoint);
							break;

						case NetIncomingMessageType.VerboseDebugMessage:
						case NetIncomingMessageType.DebugMessage:
						case NetIncomingMessageType.WarningMessage:
						
                        case NetIncomingMessageType.ErrorMessage:
                            Console.WriteLine(im.ReadString());
                            break;

						case NetIncomingMessageType.StatusChanged:
                            NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();
							
                            // Um novo jogador se conectou
                            if (status == NetConnectionStatus.Connected)
                                Console.WriteLine("Cliente " + NetUtility.ToHexString(im.SenderConnection.RemoteUniqueIdentifier) + " conectado.");

							break;

                        // Chegou input através de algum cliente
						case NetIncomingMessageType.Data:
							float xinput = im.ReadFloat();
                            float yinput = im.ReadFloat();
                            string nick = im.ReadString();

                            Console.WriteLine("Nick: " + nick + " | X: " + xinput + " | Y: " + yinput);

                            //ClientInfo ci = im.SenderConnection.Tag as ClientInfo;
                            //ci = new ClientInfo(nick, xinput, yinput);			
                            float[] pos = im.SenderConnection.Tag as float[];
                            pos[0] = xinput;
                            pos[1] = yinput;
                            
							break;
					}

                    // Envia atualizações 30x/s
					double now = NetTime.Now;
					if (now > nextSendUpdates)
					{
						// Para cada jogador...
						foreach (NetConnection player in server.Connections)
						{
							// ... envio suas informações para os outros jogadores (incluindo a si mesmo)
							foreach (NetConnection otherPlayer in server.Connections)
							{
                                NetOutgoingMessage om = server.CreateMessage();

								// Escreve de quem se trata essa posição
                                // RemoteUniqueIdentifier é um long, que representa o id do Cliente
								om.Write(otherPlayer.RemoteUniqueIdentifier);

                                if (otherPlayer.Tag == null)
                                    otherPlayer.Tag = new float[2];

                                float[] pos = otherPlayer.Tag as float[];
                                om.Write(pos[0]);
                                om.Write(pos[1]);
                                
                                // Ainda preciso entender por que criar um objeto vazio caso seja null.
                                //if (otherPlayer.Tag == null)
                                    //otherPlayer.Tag = new ClientInfo();

                                // Obtendo através da Tag os dados gravados do cliente 
                                // assim que ele enviou uma mensagem
                                //ClientInfo ci = otherPlayer.Tag as ClientInfo;
                                //om.Write(ci.nickname);
                                //om.Write(ci.xPosition);
                                //om.Write(ci.yPosition);                                			
                                
								// Envia mensagem
								server.SendMessage(om, player, NetDeliveryMethod.Unreliable);
							}
						}

                        // Agenda a próxima atualização
						nextSendUpdates += (1.0 / 30.0);
					}
				}

                // Sleep para que outros processos rodem na moral
				Thread.Sleep(1);
			}

			server.Shutdown("Desligando Servidor.");
		}
	}

    class ClientInfo
    {
        public float xPosition { get; set; }
        public float yPosition { get; set; }
        public string nickname { get; set; }

        public ClientInfo() { }

        public ClientInfo(string nick, float xPos, float yPos)
        {
            this.nickname = nick;
            this.xPosition = xPos;
            this.yPosition = yPos;
        }

    }

}
