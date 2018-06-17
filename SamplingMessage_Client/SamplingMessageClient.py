import json

class SamplingMessageClient:

    def send_request(self, connection, argparse_results):
        self.connection = connection
        self.argparse_results = argparse_results
        self.request_header = {'Content-type': 'application/json'}

        self.message_name =  self.list_to_string(self.argparse_results.message_name)

        # Call functions mapped to user input
        self.argparse_results.func()

    # Handle user input
    def create_message(self):
        message_data = json.dumps({"name": self.message_name, "validity_time": self.argparse_results.validity})
        self.connection.request("POST", "/sampling_message", message_data, self.request_header)
        self.print_response()

    def write_message(self):
        message_content = self.list_to_string(self.argparse_results.message_content)
        message_data = json.dumps({"message_content": message_content})
        self.connection.request("PUT", "/sampling_message/"+self.message_name, message_data, self.request_header)
        self.print_response()

    def clear_message(self):
        message_data = json.dumps({"message_content": "None"})
        self.connection.request("PATCH", "/sampling_message/"+self.message_name, message_data, self.request_header)
        self.print_response()

    def read_content(self):
        self.connection.request("GET", "/sampling_message/content/"+self.message_name)
        self.print_response(readContent=True)

    def read_status(self):
        self.connection.request("GET", "/sampling_message/status/"+self.message_name)
        self.print_response(readStatus=True)

    def delete_message(self):
        self.connection.request("DELETE", "/sampling_message/"+self.message_name)
        self.print_response()

    def print_response(self, readContent=False, readStatus=False):
        r1 = self.connection.getresponse()
        print("Server response code: '" + str(r1.status) + "'\nServer response reason: '" + str(r1.reason) + "'")
        if r1.status >= 400:
            return
        if readContent or readStatus:
            try:
                # print(json.loads(r1.read()))
                json_object = json.loads(r1.read())
                print("Message valid: '" + json_object["validity"] + "'")
                if readContent:
                    print("Message content: '" + json_object["message_content"] + "'")
                elif readStatus:
                    print("Message status: '" + json_object["message_status"] + "'")
                else:
                    print("Error: Unplausible program state")
                    return
            except json.decoder.JSONDecodeError:
                print("Error: Unexpected decoding failure. Body data may be missing.")

    def list_to_string(self, list):
        string = ""
        if len(list) > 1:
            for item in list:
                string += " " + item
        else:
            string = list[0]
        return string