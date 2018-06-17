import http.client
import argparse
import socket
from SamplingMessageClient import SamplingMessageClient
import traceback

def main():
    # Create SamplingMessageClient object
    message_client = SamplingMessageClient()

    # Create Argparser object
    parser = argparse.ArgumentParser(description="Send and receive Sampling Messages.")

    # Add main parsers
    parser.add_argument("--server", required=True, action="store", dest="server_address", metavar="SERVER_ADDRESS:PORT")
    subparsers = parser.add_subparsers()

    # Add "create message"-parser
    parser_create = subparsers.add_parser("create", help="Create Sampling message with timestamp in full seconds.")
    parser_create.add_argument("-n", "-name", action="store", dest="message_name", nargs="+", required=True)
    parser_create.add_argument("-v", "-validity", action="store", dest="validity", required=True, type=int, default=10,
                               metavar="VALIDITY_TIME_IN_FULL_SECONDS.")
    parser_create.set_defaults(func=message_client.create_message)

    # Add "write message"-parser
    parser_write = subparsers.add_parser("write", help="Write content to an existing message.")
    parser_write.add_argument("-n", "-name", action="store", dest="message_name", nargs="+", required=True)
    parser_write.add_argument("-c", "-content", action="store", dest="message_content", nargs="+", required=True,
                              help="Message content data.")
    parser_write.set_defaults(func=message_client.write_message)

    # Add "clear message"-parser
    parser_clear = subparsers.add_parser("clear", help="Clear content of an existing message.")
    parser_clear.add_argument("-n", "-name", action="store", dest="message_name", nargs="+", required=True)
    parser_clear.set_defaults(func=message_client.clear_message)

    # Add "read content"-parser
    parser_content = subparsers.add_parser("read", help="Read content of an existing message.")
    parser_content.add_argument("-n", "-name", action="store", dest="message_name", nargs="+", required=True)
    parser_content.set_defaults(func=message_client.read_content)

    # Add "read status"-parser
    parser_status = subparsers.add_parser("status", help="Read status of an existing message.")
    parser_status.add_argument("-n", "-name", action="store", dest="message_name", nargs="+", required=True)
    parser_status.set_defaults(func=message_client.read_status)

    # Add "delete message"-parser
    parser_delete = subparsers.add_parser("delete", help="Delete existing message.")
    parser_delete.add_argument("-n", "-name", action="store", dest="message_name", nargs="+", required=True)
    parser_delete.set_defaults(func=message_client.delete_message)

    try:

        # Parse results
        results = parser.parse_args()
        # Create HTTP-Connection object
        server_name, server_port = results.server_address.split(":")
        connection = http.client.HTTPConnection(server_name, server_port, timeout=10)

        # Send http request according to user input arguments
        message_client.send_request(connection, results)
    except ValueError:
        print("Missing Server IP or port.")
        parser.print_help()
    except argparse.ArgumentError:
        print("Error: Missing positional arguments!\n")
        #parser.print_usage()
        parser.print_help()
    except socket.timeout:
        print("Error: Connection Timeout.")
    except ConnectionRefusedError:
        print("Error: Server refused connection.")
    except Exception as exception:
        print("Error occured: '" + str(exception).upper()+"'")

# Program entry point
if __name__ == '__main__':
    main()