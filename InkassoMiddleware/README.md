# Inkasso AI Middleware System

## Overview
This is the Inkasso AI Middleware system that provides intelligent document processing and customer interaction capabilities through a Model Context Protocol (MCP) server architecture.

## Prerequisites

- Node.js 18.x or higher
- npm 8.x or higher
- Git

## Installation

1. Clone the repository:
```bash
git clone <repository-url>
cd InkassoMiddleware
```

2. Install dependencies:
```bash
npm install
```

## Configuration

1. Create a `.env` file in the root directory with the following variables:
```env
# Server Configuration
PORT=3000
NODE_ENV=development

# Add any additional configuration variables as needed
```

## Building the Application

1. Build the TypeScript code:
```bash
npm run build
```

## Running the Application

### Development Mode
To run the application with hot-reload for development:
```bash
npm run dev
```

### Production Mode
To run the built application:
```bash
npm start
```

## Project Structure

```
InkassoMiddleware/
├── src/                    # Source code
│   ├── server/            # MCP server implementation
│   ├── tools/             # Tool implementations
│   ├── services/          # Business logic services
│   └── types/             # TypeScript type definitions
├── dist/                  # Compiled JavaScript (generated)
├── package.json           # Project dependencies and scripts
├── tsconfig.json          # TypeScript configuration
└── README.md             # This file
```

## Available Scripts

- `npm run build` - Compile TypeScript to JavaScript
- `npm run dev` - Run in development mode with hot-reload
- `npm start` - Run the compiled production build
- `npm test` - Run tests (if configured)
- `npm run lint` - Run linting (if configured)

## Key Features

1. **MCP Server Architecture**: Built on the Model Context Protocol for seamless AI integration
2. **Document Processing**: Intelligent handling of various document types
3. **Customer Interaction Management**: Sophisticated tools for managing customer communications
4. **Extensible Tool System**: Easy to add new capabilities through the tool interface

## Architecture Overview

The system is built as an MCP (Model Context Protocol) server that exposes various tools for:
- Document analysis and processing
- Customer data management
- Communication handling
- Payment processing integration

## Testing

To run the test suite:
```bash
npm test
```

## Troubleshooting

### Common Issues

1. **Port already in use**: If you see an error about port 3000 being in use, either:
   - Change the PORT in your .env file
   - Stop the other process using port 3000

2. **Module not found errors**: Run `npm install` to ensure all dependencies are installed

3. **TypeScript compilation errors**: Run `npm run build` to see detailed error messages

## Support

For questions or issues, please contact the development team.

## License

Proprietary - Inkasso GmbH