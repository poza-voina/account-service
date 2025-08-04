KEYCLOAK_BIN=/opt/keycloak/bin/kcadm.sh

set -a
source /opt/keycloak/.env
set +a

$KEYCLOAK_BIN config credentials --server http://keycloak:$KEYCLOAK_PORT --realm master --user $KEYCLOAK_ADMIN_LOGIN --password $KEYCLOAK_ADMIN_PASSWORD

$KEYCLOAK_BIN create realms -s realm=$REALM -s enabled=true

echo '{
  "clientId": "'"$CLIENT_ID"'",
  "secret": "'"$CLIENT_SECRET"'",
  "enabled": true,
  "protocol": "openid-connect",
  "publicClient": false,
  "directAccessGrantsEnabled": true,
  "rootUrl": "http://localhost:'"$ACCOUNT_SERVICE_API_PORT"'",
  "baseUrl": "/",
  "redirectUris": ["http://localhost:'"$ACCOUNT_SERVICE_API_PORT"'/*"],
  "webOrigins": ["http://localhost:'"$ACCOUNT_SERVICE_API_PORT"'"],
  "attributes": {
    "post.logout.redirect.uris": "http://localhost:'"$ACCOUNT_SERVICE_API_PORT"'"
  }
}' | $KEYCLOAK_BIN create clients -r $REALM -f -

$KEYCLOAK_BIN create roles -r application-realm -s name=offline_access
$KEYCLOAK_BIN create roles -r application-realm -s name=uma_authorization

$KEYCLOAK_BIN create users -r application-realm -s username=$USERNAME -s enabled=true

USER_ID=$($KEYCLOAK_BIN get users -r application-realm -q username=$USERNAME --fields id --format csv | tail -n1 | tr -d '"')

$KEYCLOAK_BIN set-password -r application-realm --userid $USER_ID --new-password testpassword

$KEYCLOAK_BIN add-roles -r application-realm --uusername $USERNAME --rolename offline_access
$KEYCLOAK_BIN add-roles -r application-realm --uusername $USERNAME --rolename uma_authorization
