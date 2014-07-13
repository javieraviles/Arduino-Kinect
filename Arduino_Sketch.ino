#include <SPI.h>
#include <WiFi.h>


char ssid[] = "SSID Name";      // your network SSID (name) 
char pass[] = "Password";
int keyIndex = 0;                 // your network key Index number (needed only for WEP)
int back1 = 2;
int back2 = 5;
int forward1=3;
int forward2=6;
int status = WL_IDLE_STATUS;
int correcto = 1;
WiFiServer server(80);

void setup() {
  //Initialize serial and wait for port to open:
  Serial.begin(9600); 
  while (!Serial) {
    ; // wait for serial port to connect. Needed for Leonardo only
  }
  
  // check for the presence of the shield:
  if (WiFi.status() == WL_NO_SHIELD) {
    Serial.println("WiFi shield not present"); 
    // don't continue:
    while(true);
  } 
  
  // attempt to connect to Wifi network:
  while ( status != WL_CONNECTED) { 
    Serial.print("Attempting to connect to SSID: ");
    Serial.println(ssid);
    // Connect to WPA/WPA2 network. Change this line if using open or WEP network:    
    status = WiFi.begin(ssid, pass);

    // wait 10 seconds for connection:
    delay(10000);
  } 
  server.begin();
  // you're connected now, so print out the status:
  printWifiStatus();
}


void loop() {
  // listen for incoming clients
  WiFiClient client = server.available();
  String recibidos = "";
  if (client) {
    Serial.println("new client");
    // an http request ends with a blank line
    boolean currentLineIsBlank = true;
    while (client.connected()) {
      if (client.available()) {
        char c = client.read();
        Serial.write(c);
        recibidos += c;
        // if you've gotten to the end of the line (received a newline
        // character) and the line is blank, the http request has ended,
        // so you can send a reply
        if (c == '\n' && currentLineIsBlank) {
          if(recibidos.startsWith("LEF"))
          {
            Serial.println("Socket Request, left");
            client.write(correcto);
            
            digitalWrite(back1,LOW);
            digitalWrite(back2, LOW);
            digitalWrite(forward1,LOW);
            delay(100);
            digitalWrite(forward2, HIGH);
          }
          
          else if(recibidos.startsWith("RIG"))
          {
            Serial.println("Socket Request, right");
            client.write(correcto);
            
            digitalWrite(back1,LOW);
            digitalWrite(back2, LOW);
            digitalWrite(forward2, LOW);
            delay(100);
            digitalWrite(forward1,HIGH);
            
          }
          
          else if(recibidos.startsWith("UPP"))
          {
            Serial.println("Socket Request, forward");
            client.write(correcto);
            
            digitalWrite(back1,LOW);
            digitalWrite(back2, LOW);
            delay(100);
            digitalWrite(forward1,HIGH);
            digitalWrite(forward2, HIGH);
          }
          
          else if(recibidos.startsWith("BAC"))
          {
            Serial.println("Socket Request, back");
            client.write(correcto);
            
            digitalWrite(forward1,LOW);
            digitalWrite(forward2, LOW);
            delay(100);
            digitalWrite(back1,HIGH);
            digitalWrite(back2, HIGH);
            
          }
          
          else if(recibidos.startsWith("STO"))
          {
            Serial.println("Socket Request, stop");
            client.write(correcto);
            
            digitalWrite(back1,LOW);
            digitalWrite(back2, LOW);
            digitalWrite(forward1,LOW);
            digitalWrite(forward2, LOW);
          }
          
          else if (recibidos.startsWith("GET"))
          {
            Serial.println("GET Request: from navigator");
            // send a standard http response header
            client.println("HTTP/1.1 200 OK");
            client.println("Content-Type: text/html");
            client.println("Connection: close");
            client.println();
            client.println("<!DOCTYPE HTML>");
            client.println("<html>");
            // add a meta refresh tag, so the browser pulls again every 5 seconds:
            //client.println("<meta http-equiv=\"refresh\" content=\"5\">");
            // output the value of each analog input pin
            client.println("<h1>Hola mundo</h1>");
            client.println("</html>");
            break;
          }
        }
        if (c == '\n') {
          // you're starting a new line
          currentLineIsBlank = true;
        } 
        else if (c != '\r') {
          // you've gotten a character on the current line
          currentLineIsBlank = false;
        }
      }
    }
    // give the web browser time to receive the data
    delay(1);
      // close the connection:
      recibidos = "";
      client.stop();
      Serial.println("client disonnected");
  }
}


void printWifiStatus() {
  // print the SSID of the network you're attached to:
  Serial.print("SSID: ");
  Serial.println(WiFi.SSID());

  // print your WiFi shield's IP address:
  IPAddress ip = WiFi.localIP();
  Serial.print("IP Address: ");
  Serial.println(ip);

  // print the received signal strength:
  long rssi = WiFi.RSSI();
  Serial.print("signal strength (RSSI):");
  Serial.print(rssi);
  Serial.println(" dBm");
}

